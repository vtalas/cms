///<reference path="../angular-1.0.0rc7.js"/>


function xxx($scope, $http) {
	var self = this;

	$scope.aaa = function (item) {
		if (item.Type == "text") {
			return item.Content + " text";
		}
		if (item.Type == "bla") {
			return item.Content + " bla ";
		}
		return "neznamy ..";

	};

}
