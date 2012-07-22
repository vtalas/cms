function _newitem(line){
    var newitem = { Id:0, Width: 12, Type: "text", Line: line, Edit:0 };
    return newitem;
}


var GridPageCtrl = ['$scope', '$http', '$routeParams', 'appSettings', "GridApi", function ($scope, $http, $routeParams, appSettings, GridApi) {
	$scope.data = {};

	var params = {applicationId: appSettings.Id,id: $routeParams.Id};

	$scope.$on("refreshgrid", function () {
		console.log("refreshgrid ");
		$scope.getGrid();
	});

	$scope.getGrid = function () {
		console.log ("ljkasbndlkasnd")
        GridApi.getGrid(params, function (data) {
			$scope.data = data;
			var newitem = _newitem($scope.data.Lines.length);
			$scope.data.Lines.push([newitem]);
		});
	};

	$scope.getGrid();

} ]