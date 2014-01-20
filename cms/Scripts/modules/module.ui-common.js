"use strict";
angular.module('common',[])
	.directive("ngcEditableList", [function () {

		return	{
			scope: {ngcEditableList: "=" },
			link: function (scope, element, attr) {


				scope.addNewGroup = function () {
					console.log("kjbasdkjbasjd");
				}
			}
			
		};
	}]);
