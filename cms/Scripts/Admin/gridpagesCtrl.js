var gridpagesCtrl = ['$scope', '$http', '$rootScope', 'appSettings', 'GridApi', function ($scope, $http, $rootScope, appSettings, GridApi) {

	$scope.$on("setCultureEvent", function () {
		console.log("gridpagesCtrl set culture");
	});

	$scope.data = GridApi.grids({ applicationId: appSettings.Id }, function (d) {

	});

	$scope.getLink = function (item) {
		switch (item.Category) {
			case "Page":
				return "page/" + item.Id;
			case "Menu":
				return "menu/" + item.Id;
		}
	};

	$scope.newitem = {};
	$scope.newitem.Category = "Page";

	$scope.addclick = function () {
		$scope.createNew = true;
	};
	$scope.addCancel = function () {
		$scope.createNew = false;
	};
	$scope.selectNewitemType = function (type, e) {
		e.preventDefault();
		$scope.newitem.Category = type;
	};

	$scope.add = function () {
		var newitem = $scope.newitem;

		$http({ method: 'POST', url: '/api/' + appSettings.Id + '/adminApi/AddGrid', data: newitem  })
			.success(function (data, status, headers, config) {
				$scope.data.push(data);
				$scope.newitem = { Category: newitem.Category };
			})
			.error(function (data, status, headers, config) {

			});
		$scope.newName = '';
	};

	$scope.remove = function (item) {
		$http({
			method: 'DELETE',
			url: '/api/' + appSettings.Id + '/adminApi/DeleteGrid',
			headers : {'Content-Type': 'application/json;charset=utf-8'},
			data: JSON.stringify(item.Id)
		})
			.success(function (data, status, headers, config) {
				var index = $scope.data.indexOf(item);
				if (index !== -1) $scope.data.splice(index, 1);
			})
			.error(function (data, status, headers, config) {
			});
	};

	$scope.save = function (item) {
		item.Edit = 0;
		$http({ method: 'POST', url: '/api/' + appSettings.Id + '/adminApi/UpdateGrid', data: item  })
			.success(function (data, status, headers, config) {
			})
			.error(function (data, status, headers, config) {
			});
	};

	$scope.edit = function (item) {
		item.Edit = 1;
		return;
	};
}]