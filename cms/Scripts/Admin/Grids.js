///<reference path="../angular-1.0.0rc7.js"/>

var module = angular.module("gridsmodule", []);

module.config(function($routeProvider) {
	$routeProvider
		.when('/list', { controller: GridListController, template: 'template/list' })
		.when('/gridpage/:Id', { controller: GridPageCtrl, template: 'template/gridpage' })
		.when('/gridpage/:Id/edit/:GridElementId', { controller: EditCtrl, template: 'template/edit' })
		.when('/new', { controller: CreateCtrl, template: 'template/new' })
		.otherwise({ redirectTo: '/list' });
});

module.directive("gridelement", function ($templateCache) {
	var elementType = "aaaa";

	if (!$templateCache.get(elementType)) {
		$.ajax({
			url: 'template/' + elementType,
			async:false, //kvuli tomuto tu neni $http
			success: function (data) {
				$templateCache.put(elementType, data);
			}
		});
	}
	var template = $templateCache.get(elementType);

	return {
		restrict: 'E',
		scope: {},
		controller: elementType,
		template: template,
		replace: true
	};
});

appName = function() {
	var e = angular.element(".gridsmodule"); ;
	return e.data("application-name");
};

function GridListController($scope, $http) {
	var self = this;
	//console.log($element);
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

function GridPageCtrl($scope, $http, $routeParams) {
	$scope.data = { };
	
	$http({ method: 'POST', url: '/adminApi/' + appName() + '/GetGrid/' + $routeParams.Id })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			//console.log(data, "edit");
		})
		
		.error(function (data, status, headers, config) {

		});

		$scope.add = function (parameters) {
			var newitem = { Width: 12, Content: $scope.newName };
			$scope.data.GridElements.push(newitem);

			$http({
				method: 'POST',
				url: '/adminApi/null/AddGridElement',
				data: {
					data: newitem,
					gridId: $scope.data.Id
				}
			}).success(function(data, status, headers, config) {

			});

			$scope.newName = '';


			console.log($scope.newName);
		};

}

function CreateCtrl($scope) {
	
}
function EditCtrl($scope, $route, $routeParams, $location, $http, $log, $templateCache) {
	console.log($route)
	console.log($routeParams)
	console.log($location)
	console.log($scope)
	console.log($log)
	console.log($templateCache)
}

