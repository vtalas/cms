var menuCtrl = ['$scope', '$http', '$rootScope', 'appSettings', 'apimenu', function ($scope, $http, $rootScope, appSettings, apimenu) {

	$scope.$on("setCultureEvent", function () {
		console.log("menuCtrl set culture")
	})


	$scope.data = apimenu.list({}, function (d) {
		console.log(d);
	});

	$scope.addclick = function () {
		$scope.createNew = true;
	};
	$scope.createNewCancel = function () {
		$scope.createNew = false;
	};


} ]