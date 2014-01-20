function ngcClickEditDirective() {
	var previewTemplate = '<span ng-click="showEdit()" class="ngc-click-edit-preview" ng-hide="edit" ng-bind="ngcClickEdit" >{{ngcClickEdit}}</span>',
		editTemplate = "<input ui-event=\"{ blur : 'showPreview()' }\" class='ngc-click-edit-input small' ng-show='edit' ng-model=\"ngcClickEdit\"></input>";

	return {
		scope: { ngcClickEdit: "=" },
		template:previewTemplate + editTemplate,
		link: function (scope, element, attrs) {

			scope.edit = false;
			element.addClass("ngc-click-edit");

			scope.$watch("edit", function (newValue, b) {
				element.find("input").focus();
			});

			scope.$watch("ngcClickEdit", function (newvalue, b) {
				var input = element.find("span");
				if (!newvalue) {
					input.text(attrs.placeholder);
					input.addClass("empty");
				}
				else {
					input.removeClass("empty");
				}
			});

			var namespace = attrs.namespace || "ngcClickEdit";
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