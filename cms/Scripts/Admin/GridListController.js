var GridListController = ['$scope', '$http', '$rootScope', 'appSettings', 'GridApi', function ($scope, $http, $rootScope, appSettings, GridApi) {

	$scope.data = GridApi.grids({ applicationId: appSettings.Id }, function (d) {
	});

	$scope.addclick = function () {
		$scope.createNew = true;
	};
	$scope.createNewCancel = function() {
		$scope.createNew = false;
	};


	$scope.add = function () {
		var newitem = $scope.newitem;

		$http({ method: 'POST', url: '/adminApi/' + appSettings.Id + '/AddGrid', data: { data: newitem} })
            .success(function (data, status, headers, config) {
            	$scope.data.push(data);
            })
            .error(function (data, status, headers, config) {

            });
		$scope.newName = '';
	};

	$scope.remove = function (item) {
		$http({ method: 'POST', url: '/adminApi/' + appSettings.Id + '/DeleteGrid', data: { id: item.Id} })
            .success(function (data, status, headers, config) {
            	var index = $scope.data.indexOf(item);
            	if (index != -1) $scope.data.splice(index, 1);
            })
            .error(function (data, status, headers, config) {

            });
	};

	$scope.save = function (item) {
		item.Edit = 0;
		$http({ method: 'POST', url: '/adminApi/' + appSettings.Id + '/UpdateGrid', data: { data: item} })
            .success(function (data, status, headers, config) {
            })
            .error(function (data, status, headers, config) {

            });
	};

	$scope.edit = function (item) {
		item.Edit = 1;
		return;
	};
} ]