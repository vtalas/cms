///<reference path="../angular-1.0.0rc8.js"/>
///<reference path="../angular-resource-1.0.0rc8.js"/>

var module = angular.module("gridsmodule", ['cmsapi']);

module.config(['$routeProvider', '$provide', function ($routeProvider, $provide) {
	$provide.factory('appSettings', function () {
		var e = angular.element(".gridsmodule"); ;
		var settings = {
			Name: e.data("application-name")
		};
		return settings;
	});
	$provide.factory('$gridApi', function () {
		return null;
	});
	$routeProvider
		.when('/list', { controller: GridListController, template: 'template/list' })
		.when('/gridpage/:Id', { controller: GridPageCtrl, template: 'template/gridpage' })
		.when('/gridpage/:Id/edit/:GridElementId', { controller: EditCtrl, template: 'template/edit' })
		.when('/new', { controller: CreateCtrl, template: 'template/new' })
		.otherwise({ redirectTo: '/list' });
}]);

module.directive("gridelement", ['$templateCache', '$compile', function ($templateCache, $compile) {
	var template = function (type) {
		var elementType = type ? type : "text";
		if (!$templateCache.get(elementType)) {
			$.ajax({
				url: 'GridElementTmpl',
				data: { type: elementType },
				async: false, //kvuli tomuto tu neni $http
				success: function (data) {
					$templateCache.put(elementType, data);
				}
			});
		}
		return $templateCache.get(elementType);
	};

	var directiveDefinitionObject = {
		scope: {},
		inject: {
			gridelement: 'accessor'
		},
		compile: function (iElement, tAttrs, transclude) {
			return function(scope, iElement, tAttrs, transclude) {
				console.log(scope);
				var item = scope.$parent.item;
				var type = item.Type;

				var sablona = template(type);
				var compiled = $compile(sablona);
				iElement.html(compiled(scope.$parent));
			};
		}
	};
	return directiveDefinitionObject;
}]);


function CreateCtrl($scope) {

}

//function EditCtrl($scope, $route, $routeParams, $location, $http, $log, $templateCache, appSettings) {
var EditCtrl = ['$scope', '$route', '$routeParams', 'location', '$http', '$log', '$templateCache', 'appSettings', function($scope, $route, $routeParams, $location, $http, $log, $templateCache, appSettings) {
	$scope.data = {};

	$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/GetGridElement/' + $routeParams.GridElementId })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data, "edit");
		})

		.error(function (data, status, headers, config) {

		});
}]

