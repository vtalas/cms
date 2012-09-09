var menusCtrl = function ($scope, $http, $rootScope, appSettings, apimenu, GridApi) {
	menusCtrl.$inject = ["$scope", "$http", "apimenu", "appSettings"];

	$scope.$on("setCultureEvent", function () {
		console.log("menuCtrl set culture");
	});

	$scope.data = apimenu.list({ }, function (d) {
		console.log(d);
	});
	$scope.addclick = function () {
		$scope.createNew = true;
	};
	$scope.createNewCancel = function () {
		$scope.createNew = false;
	};
};

var menuCtrl = function ($scope, $http, $rootScope, appSettings, apimenu, $routeParams, GridApi) {
	menuCtrl.$inject = ["$scope", "$http", "apimenu", "appSettings", "$routeParams"];


	$scope.$on("setCultureEvent", function () {
		console.log("menuCtrl set culture");
	});

	$scope.$on("sortablxxe-dragover", function (data, item) {
		var element = $(item.element);
		element.css("border", "1px solid red");
	});

	$scope.$on("drop", function (data, item) {
		console.log(item);
	});

	$scope.showAdd = function (item) {
		item.showAdd = 1;
	};
	$scope.hideAdd = function (item) {
		item.showAdd = 0;
	};
	$scope.grids = GridApi.grids({}, function (data) {

	});
	$scope.add = function (item) {
		console.log("add", item);
		var menuitem = {
			Line: 0,
			ParentId: item.Id,
			Type: "gridpagereference"
		};

	};


	var params = { id: $routeParams.Id };

	$scope.getGrid = function () {
		$http({ method: 'GET', url: '/content/test/menu2.json' }).
			success(function (data, status, headers, config) {
				$scope.data = data;
			});
		return;
		apimenu.get(params, function (data) {
			$scope.data = data;
			console.log(data);
			//var newitem = _newitem($scope.data.Lines.length);
			//$scope.data.Lines.push([newitem]);
		});
	};

	$scope.getGrid();


};