///<reference path="../angular-1.0.0rc7.js"/>

var module = angular.module("gridsmodule", []);

module.config(function ($routeProvider) {
	$routeProvider
      .when('/list', { controller: GridListController, template: 'template/list' })
      .when('/edit/:gridId', { controller: EditCtrl, template: 'template/edit' })
      .when('/new', { controller: CreateCtrl, template: 'template/new' })
      .otherwise({ redirectTo: '/list' });
});

//console.log(module,"module");

function GridListController($scope, $http) {
	var self = this;
	var e = angular.element(".gridsmodule");;
	var appName = e.data("application-name");
	
	$http({ method: 'POST', url: '/adminApi/'+appName+'/grids' })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data);
		})

		.error(function (data, status, headers, config) {

		});

		$scope.edit = function (parameters) {
			console.log(parameters);
		};

}

function EditCtrl($scope) {
}

function CreateCtrl($scope) {
	
}

