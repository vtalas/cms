function ngcGroups() {
	return {
		restrict: "E",
		templateUrl: "template/aaa",
		link : function (scope) {
			scope.data  = ["aaa", "bb", "ccc"]
		}
	};
};
