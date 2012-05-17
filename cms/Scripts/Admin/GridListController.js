var GridListController = ['$scope', '$http', '$rootScope', 'appSettings', 'GridApi', function($scope, $http, $rootScope, appSettings, GridApi) {
	$scope.data = GridApi.get(null, function(d){
        console.log(d);
    });

    $scope.add = function () {
        var newitem = {Name : $scope.newName};
	    $http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/AddGrid', data: {data : newitem} })
            .success(function (data, status, headers, config) {
                $scope.data.push(data);
            })
            .error(function (data, status, headers, config) {

            });
        $scope.newName = '';
    };
    $scope.remove = function (item) {

    	$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/DeleteGrid', data: { id: item.Id} })
            .success(function (data, status, headers, config) {
                var index = $scope.data.indexOf(item);
                if (index != -1) $scope.data.splice(index, 1);
            })
            .error(function (data, status, headers, config) {

            });
    };
}]