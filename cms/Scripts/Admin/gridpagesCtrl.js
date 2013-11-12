var gridpagesCtrl = ['$scope', '$http', '$rootScope', 'appSettings', 'GridApi',  "$json",
	function ($scope, $http, $rootScope, appSettings, GridApi, $json) {

	var x;


	function loadData() {
		GridApi.load().success(function (data) {
			x = new Picus(data, $json);
			$scope.data = x.data;
		});
	}
	function saveChanges(callback) {
		GridApi.save($scope.data, callback);
	}

	$scope.$on("setCultureEvent", function () {
		$scope.data = null;
		loadData();
	});

	loadData();

	$scope.aaa = function (item) {
		return "/clientApi/" + appSettings.Id + "/getpage/" + item.Link;
	};

	$scope.getLink = function (item) {
		switch (item.Category) {
			case "Page":
				return "page/" + item.id;
			case "Menu":
				return "menu/" + item.id;
		}
	};

	$scope.remove = function (item) {
		x.remove(item);
	};

	$scope.save = function (item) {
		item.Edit = 0;
		x.update(item);
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


		x.addGrid(newitem);
		$scope.newitem = { Category: newitem.Category };
		$scope.newItemEdit = false;

		/*$http({ method: 'POST', url: '/api/' + appSettings.Id + '/adminApi/AddGrid', data: newitem })
			.success(function (data, status, headers, config) {
				$scope.data.push(data);
				$scope.newitem = { Category: newitem.Category };
				$scope.newItemEdit = false;
			})
			.error(function (data, status, headers, config) {
			    var error = {
			        message: data.ExceptionMessage,
			        type: "error"
			    };
			    $scope.$emit("set-message", error);
			});*/
		$scope.newName = '';
	};

	$scope.$on("ngcClickEdit.showEdit", function () {
		$scope.newItemEdit = true;
	});

	$scope.$on("ngcClickEdit.showPreview", function () {
		newItemReset();
		$scope.newItemEdit = false;
	});

	$scope.authColor = function(grid) {
		return grid.Authorize ? "red" : "none";
	};

	$scope.authTooltip = function (grid) {
		return grid.Authorize ? "Autorizováno - Viditelné po přihlášení" : "Veřejné - Viditelné pro všechny";
	};
}];