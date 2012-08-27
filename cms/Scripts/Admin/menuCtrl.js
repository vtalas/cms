var menusCtrl = function($scope, $http, $rootScope, appSettings, apimenu) {
    menusCtrl.$inject = ["$scope", "$http", "apimenu", "appSettings"];

	$scope.$on("setCultureEvent", function() {
	    console.log("menuCtrl set culture");
	});


	$scope.data = apimenu.list({ }, function(d) {
		console.log(d);
	});

	$scope.addclick = function() {
		$scope.createNew = true;
	};
	$scope.createNewCancel = function() {
		$scope.createNew = false;
	};
	
};

var menuCtrl = function ($scope, $http, $rootScope, appSettings, apimenu, $routeParams) {
	menuCtrl.$inject = ["$scope", "$http", "apimenu", "appSettings", "$routeParams"]
	console.log("kasjvbdasjkbdjk")

	$scope.$on("setCultureEvent", function () {
		console.log("menuCtrl set culture");
	});

	var params = { id: $routeParams.Id };

	$scope.getGrid = function () {
		apimenu.get(params, function (data) {
			$scope.data = data;
			console.log(data);
			//var newitem = _newitem($scope.data.Lines.length);
			//$scope.data.Lines.push([newitem]);
		});
	};

	$scope.getGrid();



} 