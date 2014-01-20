function ngcClickEditTextAreaDirective() {
	var previewTemplate = '<span ng-click="showEdit()" class="ngc-click-edit-preview" ng-hide="edit" ng-bind="ngcClickEditTextarea" >{{ngcClickEditTextarea}}</span>',
		editTemplateInput = "<textarea ui-event=\"{ blur : 'showPreview()' }\" class='ngc-click-edit-textarea' ng-show=\"edit\" ng-model=\"ngcClickEditTextarea\" rows='15'></textarea>";

	return {
		scope: { ngcClickEditTextarea: "=" },
		template:previewTemplate + editTemplateInput,
		link: function (scope, element, attrs) {

			scope.edit = false;
			element.addClass("ngc-click-edit");

			scope.$watch("edit", function () {
				element.find("textarea").focus();
			});

			scope.$watch("ngcClickEditTextarea", function (newvalue, b) {
				var input = element.find("span");
				if (!newvalue) {
					input.text(attrs.placeholder);
					input.addClass("empty");
				}
				else {
					input.removeClass("empty");
				}
			});

			var namespace = attrs.namespace || "ngcClickEditTextarea";
			scope.showEdit = function () {
				scope.$emit(namespace + ".showEdit");
				scope.edit = true;
			};

			scope.showPreview = function () {
				scope.$emit(namespace + ".showPreview");
				scope.edit = false;
			};
		}
	};
}