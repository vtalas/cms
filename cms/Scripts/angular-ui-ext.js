var xxx = function (sourceItem, sourceScope,  destinationItem, destinationScope) {
	return {
		sourceItem: sourceItem,
		sourceScope: sourceScope,
		destinationItem: destinationItem,
		destinationScope: destinationScope
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
						opts[namespace].ngstart(e, options, element, xxx(modelvalues,scope, modelvalues, scope));
					}
				});
				$(element).on("dragend", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null;

					if (typeof (opts[namespace].ngdragend) === "function") {
						opts[namespace].ngdragend(e, options, scope, modelvalues, element);
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
			var validNamespaceIndex, validNamespace;
			var namespaceArray = attrs.uiDropableHtml.split(",");

			if (scope.$root.draggeditem) {
				validNamespaceIndex = namespaceArray.indexOf(scope.$root.draggeditem.namespace);
				validNamespace = namespaceArray[validNamespaceIndex];
			}
			return validNamespace;
		}

		return {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModel) {
				var opts;
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				$(element).on("dragover", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null;
					
					var namespace = getNamespace(scope, attrs);
					if (namespace && typeof opts[namespace].ngdragover === "function") {
						return opts[namespace].ngdragover(e, opts, element, xxx(scope.$root.draggeditem, scope.$root.draggedScope, modelvalues, scope));
					}
				});
				
				$(element).on("drop", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null;
					var namespace = getNamespace(scope, attrs);
					
					if (namespace && typeof opts[namespace].ngdrop === "function") {
						return opts[namespace].ngdrop(e, opts, element, xxx(scope.$root.draggeditem, scope.$root.draggedScope, modelvalues, scope));
					}
				});
			}
		};
	}
]);