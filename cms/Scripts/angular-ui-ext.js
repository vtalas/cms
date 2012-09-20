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

function registerDragEvent(dragevent, scope, namespaceArray, element, ngModel, opts) {
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

				registerDragEvent("dragenter", scope, namespaceArray, element, ngModel, opts);
				registerDragEvent("dragleave", scope, namespaceArray, element, ngModel, opts);
				registerDragEvent("dragover", scope, namespaceArray, element, ngModel, opts);
				registerDragEvent("drop", scope, namespaceArray, element, ngModel, opts);
			}
		};
	}
]);