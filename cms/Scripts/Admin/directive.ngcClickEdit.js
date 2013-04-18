function ngcClickEditDirective() {
	var previewTemplate = '<span ng-click="showEdit()" class="ngc-click-edit-preview" ng-hide="edit" ng-bind="ngcClickEdit" ></span>',
		editTemplateInput = "<input ui-event=\"{ blur : 'showPreview()' }\" class='ngc-click-edit-input' ng-show='edit' ng-model=\"ngcClickEdit\"></input>",
		editTemplateTextArea = "<textarea ui-event=\"{ blur : 'showPreview()' }\" class='ngc-click-edit-textarea' ng-show=\"edit\" ng-model=\"ngcClickEdit\" ></textarea>";
	return {
		scope: { ngcClickEdit: "=" },
		compile: function (iElement, iAttrs, transclude) {
			var preview = angular.element(previewTemplate),
				type = iAttrs.type || "input",
				template;

			iElement.html(preview);

			switch (type) {
				case "input":
					template = angular.element(editTemplateInput);
					break;
				case "textarea":
					template = angular.element(editTemplateTextArea);
					break;
				default :
					template = angular.element(editTemplateInput);
			}
			template.attr("placeholder", iAttrs.placeholder);
			iElement.append(template);

			return function (scope, element, attrs) {
				scope.edit = false;

				scope.$watch("edit", function (newValue, b) {
					var input = element.find(type)[0];
					input.focus();
				});

				scope.$watch("ngcClickEdit", function (newvalue, b) {
					var input = element.find("span")[0];
					if (!newvalue) {
						$(input).text(attrs.placeholder);
						$(input).addClass("empty");
					}
					else {
						$(input).removeClass("empty");

					}
				});

				scope.showEdit = function () {
					scope.edit = true;
				};

				scope.showPreview = function () {
					scope.$emit("ngcClickEdit-showPreview");
					scope.edit = false;
				};
			};
		}
	};
}