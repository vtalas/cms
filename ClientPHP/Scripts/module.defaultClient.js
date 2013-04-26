var module = angular.module("defaultClient", ["apiModule"]);

module.factory('cache', ['$cacheFactory', function ($cacheFactory) {
	return $cacheFactory("masparti");
}]);


module.config(['$routeProvider', '$provide', function ($routeProvider) {
	$routeProvider
		.when('/home', {controller: homeController, templateUrl: 'template.home.html'})
		.otherwise({redirectTo: '/home'});
}]);

