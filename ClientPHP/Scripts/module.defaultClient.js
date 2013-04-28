var module = angular.module("defaultClient", ["apiModule"]);

module.factory('cache', ['$cacheFactory', function ($cacheFactory) {
	return $cacheFactory("masparti");
}]);

module.config(['$routeProvider', '$provide', function ($routeProvider) {
	$routeProvider
		.when('/page/:link', {controller: pageController, templateUrl: 'template.page.html'})
		.when('/home', {controller: homeController, templateUrl: 'template.home.html'})
		.otherwise({redirectTo: '/home'});
}]);

module.directive("gridelement", function ($compile, $templateCache	) {
	var directiveDefinitionObject = {
		scope: { grid: "=", gridelement: "=" },
		link: function (scope, iElement, tAttrs, controller) {
			var template = $templateCache.get(scope.gridelement.Type + ".thtml");
			var compiled = $compile(template)(scope);
			iElement.html(compiled);
		}
	};
	return directiveDefinitionObject;
});




