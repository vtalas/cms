var DRAGGED = 1,
	PRD = 0,
	DRAGEND = 5,
	DRAGOLD = 9,
	DROPPED = 3,
	PSEUDOHIDDEN = 333,
	SWAPPED = 2;

angular.module('ui').value("$draggeditem", { source: {}, destination: {} });
angular.module('ui.directives').directive('uiDraganddropHtml', ['ui.config', '$draggeditem', function (uiConfig, $draggeditem) {
	var options;
	options = {};
	if (uiConfig.draganddrophtml !== null) {
		angular.extend(options, uiConfig.draganddrophtml);
	}

	return {
		require: '?ngModel',
		link: function (scope, element, attrs, ngModel) {
			var namespaceArray = attrs.uiDraganddropHtml.split(","),
			    namespace = namespaceArray[0],
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

			(element).attr("draggable", true);

			scope.$watch("item.status", function (newval, oldval) {
				scope.$emit("statuschange-sortablehtml", { oldvalue: oldval, newvalue: newval, element: element, item: ngModel.$modelValue });
			});

			$(element).on("dragstart", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null,
					itemcopy = angular.extend({}, modelvalues);

				var clone = $(element).clone();
				$(clone).removeAttr("ui-draganddrop-html");
				$(clone).removeAttr("ng-model");
				$(clone).removeAttr("ng-show");
				//console.log(clone)
				//$(".xxx").append($(element).clone())


				$draggeditem.source.item = modelvalues;
				//$draggeditem.source.item = itemcopy;
				$draggeditem.source.scope = scope;
				$draggeditem.source.element = clone;
				$draggeditem.source.namespace = namespace;
				$draggeditem.destination = $draggeditem.source;

				if (typeof (opts[namespace].dragstart) === "function") {
					opts[namespace].dragstart(e, options, $draggeditem);
				}
				//modelvalues.status = DRAGOLD;
			});

			(element).bind("dragenter", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].dragenter) === "function") {
					$draggeditem.destination = { item: modelvalues, scope: scope, element: element };
					opts[namespace].dragenter(e, options, element, $draggeditem);
				}
			});
			(element).bind("dragleave", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;
				if (typeof (opts[namespace].dragleave) === "function") {
					$draggeditem.destination = { item: modelvalues, scope: scope, element: element };
					opts[namespace].dragleave(e, options, element, $draggeditem);
				}
			});
			(element).bind("dragover", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].dragover) === "function") {
					$draggeditem.destination = { item: modelvalues, scope: scope, element: element };
					opts[namespace].dragover(e, options, element, $draggeditem);
				}
			});
			(element).bind("drop", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].drop) === "function") {
					$draggeditem.destination = { item: modelvalues, scope: scope, element: element };
					opts[namespace].drop(e, options, element, $draggeditem);
				}
			});
			//dragend nevi o koncovym elementu, proto se destination neupdatuje
			(element).bind("dragend", function (e) {
				if (typeof (opts[namespace].dragend) === "function") {
					opts[namespace].dragend(e, options, element, $draggeditem);
				}
			});



		}
	};
}
]);



var xxx = function (source, destination, namespace) {
	return {
		sourceItem: source.item,
		sourceScope: source.scope,
		sourceElement: source.element,
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