function _newitem(position) {
	var newitem = { Id: 0, Width: 12, Type: "text", Position: position, Edit: 0 };
	return newitem;
}

var pageCtrl = ['$scope', '$http', '$routeParams', 'appSettings', "GridApi", function ($scope, $http, $routeParams, appSettings, GridApi) {
	$scope.data = {};

	var params = { applicationId: appSettings.Id, id: $routeParams.Id };

	$scope.newItem = {
		Id: 0,
		Type: "text",
		Edit: 0,
		Width:12,
	};

	$scope.$on("refreshgrid", function () {
		console.log("refreshgrid ");
		$scope.getGrid();
	});

	$scope.$on("setCultureEvent", function () {
		console.log("pageCtrl set culture");
		$scope.getGrid();
	});


	$scope.getGrid = function () {
		GridApi.getGrid(params, function (data) {
			$scope.data = data;
		});
	};



	$scope.getGrid();

}];