var pathPrefix = "/Content/application/groups/";
var module = angular.module("editableElements", []);

/**
 *
 */
"use strict";
module.directive("ngcEditableList", ngcEditableList);
/**
 *
 */
module.directive("ngcClickEdit", ngcClickEditDirective);

module.directive("ngcClickEditInput", ngcClickEditInputDirective);

module.directive("ngcClickEditTextarea", ngcClickEditTextAreaDirective);


function ngcEditableList() {
	return {
		scope: {
			ngcEditableList: "="
		},
		templateUrl: pathPrefix + "template.groups.html",
		link: function (scope, element, attrs) {
			scope.newGroupName = "";
			scope.addNewGroup = function () {
				var groups = scope.ngcEditableList,
					i,
					id = 0;

				for (i = 0; i < groups.length; i++) {
					id = Math.max(id, groups[i].id);
				}
				id++;
				groups.push({id: id, name: scope.newGroupName});
				scope.$emit("trigger-save");
				scope.newGroupName = "";
			};

			scope.deleteGroup = function (id) {
				var groups = scope.ngcEditableList,
					i;

				for (i = 0; i < groups.length; i++) {
					if (id === groups[i].id) {
						groups.splice(i, 1);
						scope.$emit("trigger-save");
						return;
					}
				}
			};
			scope.$on("ngcClickEditInput.showPreview", function () {
				scope.$emit("trigger-save");
			});
		}
	};
}

function ngcClickEditInputDirective() {
	var previewTemplate = '<span ng-click="showEdit()"  class="ngc-click-edit-preview" ng-hide="edit" ng-bind="ngcClickEditInput" >{{ngcClickEdit}}</span>',
		editTemplate = "<input ui-event=\"{ blur : 'showPreview()'}\"  ui-keypress=\"{13:'onEnter($event)'}\" class='ngc-click-edit-input small' ng-show='edit' ng-model=\"ngcClickEditInput\" />";

	return {
		scope: {
			ngcClickEditInput: "=",
		},
		template: previewTemplate + editTemplate,
		link: function (scope, element, attrs) {
			scope.edit = false;

			element.addClass("ngc-click-edit");

			scope.$watch("edit", function (newValue, b) {
				element.find("input").focus();
			});

			scope.$watch("ngcClickEditInput", function (newvalue, b) {
				var input = element.find("span");
				if (!newvalue) {
					input.text(attrs.placeholder);
					input.addClass("empty");
				}
				else {
					input.removeClass("empty");
				}
			});

			var namespace = attrs.namespace || "ngcClickEditInput";
			scope.showEdit = function () {
				scope.$emit(namespace + ".showEdit");
				scope.edit = true;
			};

			scope.showPreview = function () {
				scope.$emit(namespace + ".showPreview");
				scope.edit = false;
			};

			scope.onEnter = function (e) {
				e.preventDefault();
				scope.edit = false;
			};
		}
	};
}

function ngcClickEditDirective() {
	var previewTemplate = '<span ng-click="showEdit()" class="ngc-click-edit-preview" ng-hide="edit" ng-bind="ngcClickEdit" >{{ngcClickEdit}}</span>',
		editTemplate = "<input ui-event=\"{ blur : 'showPreview()' }\" class='ngc-click-edit-input small' ng-show='edit' ng-model=\"ngcClickEdit\"></input>";

	return {
		scope: { ngcClickEdit: "=" },
		template: previewTemplate + editTemplate,
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

function ngcClickEditTextAreaDirective() {
	var previewTemplate = '<span ng-click="showEdit()" class="ngc-click-edit-preview" ng-hide="edit" ng-bind="ngcClickEditTextarea" >{{ngcClickEditTextarea}}</span>',
		editTemplateInput = "<textarea ui-event=\"{ blur : 'showPreview()' }\" class='ngc-click-edit-textarea' ng-show=\"edit\" ng-model=\"ngcClickEditTextarea\" rows='15'></textarea>";

	return {
		scope: { ngcClickEditTextarea: "=" },
		template: previewTemplate + editTemplateInput,
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