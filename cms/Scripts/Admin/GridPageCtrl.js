function _newitem(line){
    var newitem = { Id:0, Width: 12, Type: "text", Line: line, Edit:0 };
    return newitem;
}


var GridPageCtrl = ['$scope', '$http', '$routeParams', 'appSettings', "GridApi", function ($scope, $http, $routeParams, appSettings,GridApi) {
	$scope.data = {};

    var params = {
        applicationId : appSettings.Id,
        id:$routeParams.Id
    };
    GridApi.getGrid(params, function (data) {
        $scope.data = data;
        var newitem = _newitem($scope.data.Lines.length);
        $scope.data.Lines.push([newitem]);
    });

	$scope.add = function (item) {

        GridApi.AddGridElement({
            applicationId : appSettings.Id,
            data: item,
            gridId: $scope.data.Id
        }, function (data) {
            if (data.Line >= $scope.data.Lines.length-1) {
                var newitem = _newitem($scope.data.Lines.length);
                $scope.data.Lines.push([newitem]);
            }
            data.Edit = 1;
            $scope.data.Lines[data.Line][data.Position] = data;
        });
	};
	$scope.remove = function (item) {
        GridApi.DeleteGridElement({applicationId : appSettings.Id,data: item},
        function (data) {
            console.log("deleted");
            item.Id= 0;item.Edit =0;item.Content ="";
        });
	};

	$scope.edit = function (item, $element) {
		item.Edit = 1;
	};

	$scope.save = function (item) {
		var data = jQuery.extend(true, {}, item);

		if (angular.isObject(data.Content))
			data.Content = JSON.stringify(data.Content);

        GridApi.UpdateGridElement({applicationId : appSettings.Id,data: data},
        function () {
            item.Edit = 0;
        });
	};
}]