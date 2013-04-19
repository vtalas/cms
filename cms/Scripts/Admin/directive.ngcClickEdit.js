function ngcClickEditDirective() {
	var previewTemplate = '<span ng-click="showEdit()" class="ngc-click-edit-preview" ng-hide="edit" ng-bind="ngcClickEdit" ></span>',
		editTemplateInput = "<input ui-event=\"{ blur : 'showPreview()' }\" class='ngc-click-edit-input small' ng-show='edit' ng-model=\"ngcClickEdit\"></input>",
		editTemplateTextArea = "<textarea ui-event=\"{ blur : 'showPreview()' }\" class='ngc-click-edit-textarea' ng-show=\"edit\" ng-model=\"ngcClickEdit\" ></textarea>";
	function getEditTemplate(iAttrs) {
		var type = iAttrs.type,
			templateEdit = iAttrs.templateEdit,
			template;

		if (templateEdit) {
			return $($(templateEdit).html());
		}

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

		return template;
	}


	return {
		scope: { ngcClickEdit: "=" },
		compile: function (iElement, iAttrs, transclude) {
			var	templatePreview = iAttrs.templatePreview,
				preview =  templatePreview ? $(templatePreview).html() : angular.element(previewTemplate),
				template;

			iElement.html(preview);

			template = getEditTemplate(iAttrs);

			template.attr("placeholder", iAttrs.placeholder);
			iElement.append(template);

			return function (scope, element, attrs) {
				scope.edit = false;
				element.addClass("ngc-click-edit");


				scope.$watch("edit", function (newValue, b) {
					console.log(template);
					template.focus();
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