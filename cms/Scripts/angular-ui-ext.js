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
						if (typeof (opts[namespace].ngstart) === "function") {
							var modelvalues = ngModel ? ngModel.$modelValue : null;
							opts[namespace].ngstart(e, options, scope, modelvalues);
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
					var opts;
					opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));
					$(element).on("dragover", function (e) {
						e.stopPropagation();
						e.preventDefault();
						//opts.ngstart(e, scope);
					});
					$(element).on("drop", function (e) {

						if (typeof opts.ngdrop === "function") {
							var modelvalues = ngModel ? ngModel.$modelValue : null;
							return opts.ngdrop(e, opts, scope, modelvalues);
						}
					});
				}
			};
		}
	]);