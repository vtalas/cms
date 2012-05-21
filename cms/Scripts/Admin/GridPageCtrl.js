function _newitem(line){
    var newitem = { Id:0, Width: 12, Type: "text", Line: line, Edit:0 };
    return newitem;
}


var GridPageCtrl = ['$scope', '$http', '$routeParams', 'appSettings', function ($scope, $http, $routeParams, appSettings) {
	$scope.data = {};
	var self = this;

	$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/GetGrid/' + $routeParams.Id })
		.success(function (data, status, headers, config) {
			$scope.data = data;
            var newitem = _newitem($scope.data.Lines.length);
            $scope.data.Lines.push([newitem]);
		})
		.error(function (data, status, headers, config) {

		});
	$scope.add = function (item) {

        console.log(item);
        $http({method: 'POST',url: '/adminApi/' + appSettings.Name + '/AddGridElement',
			data: {
				data: item,
				gridId: $scope.data.Id
			}
		}).success(function (data, status, headers, config) {
//		    console.log(data.Line , $scope.data.Lines.length-1);
			if (data.Line >= $scope.data.Lines.length-1) {
                var newitem = _newitem($scope.data.Lines.length);
                $scope.data.Lines.push([newitem]);
            }
            data.Edit = 1;
			$scope.data.Lines[data.Line][data.Position] = data;

		});
	};
	$scope.remove = function (item) {
		$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/DeleteGridElement', data: item })
				.success(function (data, status, headers, config) {
					console.log("deleted");
                    item.Id= 0;item.Edit =0;
            });
	};

	$scope.edit = function (item, $element) {
		item.Edit = 1;
	};

	$scope.save = function (item) {
		var data = jQuery.extend(true, {}, item);

		if (angular.isObject(data.Content))
			data.Content = JSON.stringify(data.Content);

		$http({
			method: 'POST',
			url: '/adminApi/' + appSettings.Name + '/UpdateGridElement/' + $scope.data.Id,
			data: data
		}).success(function () {
			item.Edit = 0;
		});
	};
}]