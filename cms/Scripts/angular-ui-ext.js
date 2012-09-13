var xxx = function (sourceItem, sourceScope,  destinationItem, destinationScope, namespace) {
	return {
		sourceItem: sourceItem,
		sourceScope: sourceScope,
		destinationItem: destinationItem,
		destinationScope: destinationScope,
		namespace : namespace
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
			modelvalues = ngModel ? ngModel.$modelValue : null;
		if (!opts[namespace]) {
			console.log("namespace is not defined", opts[namespace]);
			return;
		}
		if (typeof (opts[namespace][dragevent]) === "function") {
			obj = xxx(scope.$root.draggeditem, scope.$root.draggedScope, modelvalues, scope, namespace);
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
				var opts,
					namespace = attrs.uiDraggableHtml,

				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));
				$(element).attr("draggable", true);
				//registerDragEvent("dragend", scope, namespaceArray, element, ngModel, opts);

				$(element).on("dragstart", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null;

					scope.$root.draggeditem = modelvalues;
					scope.$root.draggedScope = scope;
					scope.$root.draggeditem.namespace = namespace;

					if (typeof (opts[namespace].dragstart) === "function") {
						opts[namespace].dragstart(e, options, element, xxx(modelvalues, scope, modelvalues, scope, namespace));
					}
				});
				$(element).on("dragend", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null;

					if (typeof (opts[namespace].dragend) === "function") {
						opts[namespace].dragend(e, options, element, xxx(modelvalues, scope, modelvalues, scope, namespace));
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
				var opts,
					namespaceArray = attrs.uiDropableHtml.split(",");

				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				registerDragEvent("dragenter", scope, namespaceArray, element, ngModel, opts);
				registerDragEvent("dragleave", scope, namespaceArray, element, ngModel, opts);
				registerDragEvent("dragover", scope, namespaceArray, element, ngModel, opts);
				registerDragEvent("drop", scope, namespaceArray, element, ngModel, opts);
			}
		};
	}
]);