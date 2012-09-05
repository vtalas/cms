angular.module('ui.directives').directive('uiDraggable', [
	'ui.config', function (uiConfig) {
		var options;
		options = {};
		if (uiConfig.draggable !== null) {
			angular.extend(options, uiConfig.draggable);
		}
		return {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModel) {
				var onStart, onUpdate, opts, _start, _update;
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				opts.start = function (e, ui) {
					var values = ngModel.$modelValue;
					console.log("drag start", opts, ngModel, scope.$root);
					scope.$root.uiDraggable = values;
					//return scope.$apply();
					return;

				};
				opts.stop = function (e, ui) {
					//var a = ngModel.$modelValue.splice(end, 0, ngModel.$modelValue.splice(start, 1)[0]);

					console.log("drag stop", e, ui, ngModel, ngModel.$modelValue);
					
					return scope.$apply();
					return;
					onStart(e, ui);
					if (typeof _start === "function") {
						_start(e, ui);
					}
					return scope.$apply();
				};

				return element.draggable(opts);
			}
		};
	}
]);
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
				var onStart, onUpdate, opts, _start, _update;
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));
				$(element).on("dragstart", function (e) {
					opts.ngstart(e, scope, ngModel.$modelValue);
				});
			}
		};
	}
]);
	angular.module('ui.directives').directive('uiDropableHtml', [
	'ui.config', function (uiConfig) {
		var options;
		options = {};
		if (uiConfig.draggablehtml !== null) {
			angular.extend(options, uiConfig.draggablehtml);
		}
		return {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModel) {
				var onStart, onUpdate, opts, _start, _update;
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));
				$(element).on("dragover", function (e) {
					e.stopPropagation();
					e.preventDefault();
					//opts.ngstart(e, scope);
				});
				$(element).on("drop", function (e) {
					if (ngModel) {
						return opts.ngdrop(e, scope, ngModel.$modelValue);
					}
					return opts.ngdrop(e, scope, null);
				});
			}
		};
	}
]);
