var module = angular.module("cmsusers", ["cmsapi"]);

module.config(['$routeProvider', '$provide', function ($routeProvider, $provide) {
    $routeProvider
		.when('/gridpages', { controller: gridpagesCtrl, templateUrl: 'template/gridpages' })
		.when('/menu/:Id', { controller: menuCtrl, templateUrl: 'template/menu' })
		.when('/menu2/:Id', { controller: menu2Ctrl, templateUrl: 'template/menu2' })
		.when('/page/:Id', { controller: pageCtrl, templateUrl: 'template/gridpage' })
		.otherwise({ redirectTo: '/gridpages' });
}]);