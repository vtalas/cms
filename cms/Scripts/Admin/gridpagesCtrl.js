var gridpagesCtrl = ['$scope', '$http', '$rootScope', 'appSettings', 'GridApi', function ($scope, $http, $rootScope, appSettings, GridApi) {

    $scope.data = {};

    function loadData() {
		$scope.data  = GridApi.grids({ applicationId: appSettings.Id });
	}

	$scope.$on("setCultureEvent", function () {
		loadData();
	});

	loadData();

	$scope.getLink = function (item) {
		switch (item.Category) {
			case "Page":
				return "page/" + item.Id;
			case "Menu":
				return "menu/" + item.Id;
		}
	};

	$scope.remove = function (item) {
		$http({
			method: 'DELETE',
			url: '/api/' + appSettings.Id + '/adminApi/DeleteGrid',
			headers: {'Content-Type': 'application/json;charset=utf-8'},
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
		GridApi.updateGrid(item, function(ok) {
			console.log(ok);
		});
	};

	$scope.edit = function (item) {
		item.Edit = 1;
		return;
	};

	$scope.editModeClass = function (item) {
		return item.Edit ? "" : "hider";
	};


	var newItemReset = function () {
		$scope.newitem = {};
		$scope.newitem.Category = "Page";
	};
	$scope.newItemEdit = false;
	newItemReset();

	$scope.newItemAdd = function () {
		var newitem = $scope.newitem;

		$http({ method: 'POST', url: '/api/' + appSettings.Id + '/adminApi/AddGrid', data: newitem  })
			.success(function (data, status, headers, config) {
				$scope.data.push(data);
				$scope.newitem = { Category: newitem.Category };
				$scope.newItemEdit = false;
			})
			.error(function (data, status, headers, config) {
				console.error("Erorrrr", data)
			});
		$scope.newName = '';
	};


	$scope.$on("ngcClickEdit.showEdit", function () {
		$scope.newItemEdit = true;
	});

	$scope.$on("ngcClickEdit.showPreview", function () {
		newItemReset();
		$scope.newItemEdit = false;
	});
}];