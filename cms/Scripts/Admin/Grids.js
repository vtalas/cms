///<reference path="../angular-1.0.0rc7.js"/>

var module = angular.module("gridsmodule", []);

module.config(function ($routeProvider) {
	$routeProvider
      .when('/list', { controller: GridListController, template: 'template/list' })
      .when('/edit/:Id', { controller: EditCtrl, template: 'template/edit' })
      .when('/new', { controller: CreateCtrl, template: 'template/new' })
      .otherwise({ redirectTo: '/list' });
});

//console.log(module,"module");
appName = function() {
	var e = angular.element(".gridsmodule"); ;
	return e.data("application-name");
};

function GridListController($scope, $http) {
	var self = this;
	
	$http({ method: 'POST', url: '/adminApi/'+appName()+'/grids' })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data, "GridListController");
		})

		.error(function (data, status, headers, config) {

		});

		$scope.edit = function (parameters) {
			console.log(parameters);
		};

}

function EditCtrl($scope, $http, $routeParams) {
	$scope.data = { };
	console.log($scope);
	
	$http({ method: 'POST', url: '/adminApi/' + appName() + '/GetGrid/' + $routeParams.Id })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data, "edit");
		})
		
		.error(function (data, status, headers, config) {

		});

	$scope.edit = function (parameters) {
		console.log(parameters);
	};

}

function CreateCtrl($scope) {
	
}

