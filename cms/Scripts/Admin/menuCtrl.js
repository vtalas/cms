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
var menuCtrl = function ($scope, $http, $rootScope, appSettings, apimenu) {
	menuCtrl.$inject = ["$scope", "$http", "apimenu", "appSettings"]

	$scope.$on("setCultureEvent", function() {
	    console.log("menuCtrl set culture");
	});


} 