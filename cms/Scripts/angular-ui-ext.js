var xxx = function (sourceItem, sourceScope,  destinationItem, destinationScope, namespace) {
	return {
		sourceItem: sourceItem,
		sourceScope: sourceScope,
		destinationItem: destinationItem,
		destinationScope: destinationScope,
		namespace : namespace
	};
};

angular.module('ui.directives').directive('uiDraggablehtml', [
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
					namespace;
				namespace = attrs.uiDraggablehtml;
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				$(element).on("dragstart", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null;

					scope.$root.draggeditem = modelvalues;
					scope.$root.draggedScope = scope;
					scope.$root.draggeditem.namespace = namespace;

					if (typeof (opts[namespace].ngstart) === "function") {
						opts[namespace].ngstart(e, options, element, xxx(modelvalues, scope, modelvalues, scope, namespace));
					}
				});
				$(element).on("dragend", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null;

					if (typeof (opts[namespace].ngdragend) === "function") {
						opts[namespace].ngdragend(e, options, element, xxx(modelvalues, scope, modelvalues, scope, namespace));
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

		function getNamespace(scope, attrs) {
			var validNamespaceIndex, validNamespace,
				namespaceArray = attrs.uiDropableHtml.split(",");

			if (scope.$root.draggeditem) {
				validNamespaceIndex = namespaceArray.indexOf(scope.$root.draggeditem.namespace);
				validNamespace = namespaceArray[validNamespaceIndex];
			}
			return validNamespace;
		}

		function registerDragEvent(dragevent, scope, attrs, element, ngModel, opts) {
			$(element).on(dragevent, function (e) {
				var namespace = getNamespace(scope, attrs),
				    obj,
					modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace][dragevent]) === "function") {
					obj = xxx(scope.$root.draggeditem, scope.$root.draggedScope, modelvalues, scope, namespace);
					opts[namespace][dragevent].call({}, e, opts, element, obj);
				}
			});
		}


		return {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModel) {
				var opts;
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				registerDragEvent("dragenter", scope, attrs, element, ngModel, opts);
				registerDragEvent("dragleave", scope, attrs, element, ngModel, opts);
				registerDragEvent("dragover", scope, attrs, element, ngModel, opts);
				registerDragEvent("drop", scope, attrs, element, ngModel, opts);
			}
		};
	}
]);