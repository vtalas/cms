var xxx = function (source, destination, namespace) {
	return {
		sourceItem: source.item,
		sourceScope: source.scope,
		sourceElement : source.element,
		destinationItem: destination.item,
		destinationScope: destination.scope,
		destinationElement: destination.element,
		namespace: namespace
	};
};

function getNamespace(scope, namespaceArray) {
	var validNamespaceIndex, validNamespace;

	if (scope.$root.draggeditem) {
		validNamespaceIndex = namespaceArray.indexOf(scope.$root.draggeditem.namespace);
		validNamespace = namespaceArray[validNamespaceIndex];
	}
	return validNamespace;
}

function registerDragEventWithNamespace(dragevent, scope, namespaceArray, element, ngModel, opts) {
	$(element).on(dragevent, function (e) {

		var namespace = getNamespace(scope, namespaceArray),
			obj,
		    source,
		    destination,
			modelvalues = ngModel ? ngModel.$modelValue : null;
		if (!opts[namespace]) {
			console.log("namespace is not defined", opts[namespace]);
			return;
		}
		if (typeof (opts[namespace][dragevent]) === "function") {
			source = { item: scope.$root.draggeditem, scope: scope.$root.draggedScope, element: scope.$root.draggedElement };
			destination = { item: modelvalues, scope: scope, element: element };
			obj = xxx(source, destination, namespace);
			opts[namespace][dragevent](e, opts, element, obj);
		}
	});

}

//function registerDragEvent(dragevent, scope, type, element, ngModel, opts, draggeditem) {
//	$(element).on(dragevent, function (e) {
//		var modelvalues = ngModel ? ngModel.$modelValue : null;

//		if (!opts[type]) {
//			console.warn("type is not defined", opts[type]);
//			return;
//		}
//		if (typeof (opts[type][dragevent]) === "function") {
//			draggeditem.destination = { item: modelvalues, scope: scope, element: element };
//			console.log(draggeditem.source.Id, draggeditem.destination.Id);
			
//			opts[type][dragevent](e, opts, element, draggeditem);
//		}
//	});
//}

angular.module('ui').value("$draggeditem", {source: {}, destination: {}});
angular.module('ui.directives').directive('uiSortableHtml', ['ui.config', '$draggeditem', function (uiConfig, $draggeditem) {
	var options;
	options = {};
	if (uiConfig.sortablehtml !== null) {
		angular.extend(options, uiConfig.sortablehtml);
	}

	return {
		require: '?ngModel',
		link: function (scope, element, attrs, ngModel) {
			var namespaceArray = attrs.uiSortableHtml.split(","),
			    namespace = namespaceArray[0],
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

			$(element).attr("draggable", true);

			$(element).on("dragstart", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				$draggeditem.source.item = modelvalues;
				$draggeditem.source.scope = scope;
				$draggeditem.source.element = element;
				$draggeditem.source.namespace = namespace;
				$draggeditem.destination = $draggeditem.source;

				if (typeof (opts[namespace].dragstart) === "function") {
					opts[namespace].dragstart(e, options, element, $draggeditem);
				}
			});

			$(element).on("dragenter", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].dragenter) === "function") {
					$draggeditem.destination = { item: modelvalues, scope: scope, element: element };
					opts[namespace].dragenter(e, options, element, $draggeditem);
				}
			});
			$(element).on("dragleave", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].dragleave) === "function") {
					$draggeditem.destination = { item: modelvalues, scope: scope, element: element };
					opts[namespace].dragleave(e, options, element, $draggeditem);
				}
			});
			$(element).on("dragover", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].dragover) === "function") {
					$draggeditem.destination = { item: modelvalues, scope: scope, element: element };
					opts[namespace].dragover(e, options, element, $draggeditem);
				}
			});
			$(element).on("drop", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].drop) === "function") {
					$draggeditem.destination = { item: modelvalues, scope: scope, element: element };
					opts[namespace].drop(e, options, element, $draggeditem);
				}
			});
			//dragend nevi o koncovym elementu, proto se destination neupdatuje
			$(element).on("dragend", function (e) {
				if (typeof (opts[namespace].dragend) === "function") {
					opts[namespace].dragend(e, options, element, $draggeditem);
				}
			});

		}
	};
}
]);

angular.module('ui.directives').directive('uiDraggableHtml', [
	'ui.config', function (uiConfig) {
		var options;
		options = {};
		if (uiConfig.draggablehtml !== null) {
			angular.extend(options, uiConfig.draggablehtml);
		}
		return {
			require: '?ngModel',

			link: function (scope, element, attrs, ngModel) {
				var namespace = attrs.uiDraggableHtml,
					opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				$(element).attr("draggable", true);

				$(element).on("dragstart", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null,
					    source, destination;

					scope.$root.draggeditem = modelvalues;
					scope.$root.draggedScope = scope;
					scope.$root.draggedElement = element;
					scope.$root.draggeditem.namespace = namespace;

					if (typeof (opts[namespace].dragstart) === "function") {
						source = { item: scope.$root.draggeditem, scope: scope.$root.draggedScope, element: scope.$root.draggedElement };
						destination = { item: modelvalues, scope: scope, element: element };
						opts[namespace].dragstart(e, options, element, xxx(source, destination, namespace));
					}
				});
				$(element).on("dragend", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null,
					    source, destination;
					if (typeof (opts[namespace].dragend) === "function") {
						source = { item: scope.$root.draggeditem, scope: scope.$root.draggedScope, element: scope.$root.draggedElement };
						destination = { item: modelvalues, scope: scope, element: element };
						opts[namespace].dragend(e, options, element, xxx(source, destination, namespace));
					}
				});
			}
		};
	}
]);
angular.module('ui.directives').directive('uiDropableHtml', [
	'ui.config', function (uiConfig) {
		var options;
		options = {};
		if (uiConfig.dropablehtml !== null) {
			angular.extend(options, uiConfig.dropablehtml);
		}

		return {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModel) {
				var namespaceArray = attrs.uiDropableHtml.split(","),
					opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				registerDragEventWithNamespace("dragenter", scope, namespaceArray, element, ngModel, opts);
				registerDragEventWithNamespace("dragleave", scope, namespaceArray, element, ngModel, opts);
				registerDragEventWithNamespace("dragover", scope, namespaceArray, element, ngModel, opts);
				registerDragEventWithNamespace("drop", scope, namespaceArray, element, ngModel, opts);
			}
		};
	}
]);