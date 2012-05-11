///<reference path="../angular-1.0.0rc8.js"/>


var module = angular.module("gridsmodule", []);
module.config(function ($routeProvider, $provide) {
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
});
module.run(function ($route) {
});

module.directive("gridelement", function factory($templateCache, $compile, $rootScope) {
	var template = function (type) {
		var elementType = type ? type : "text";
		if (!$templateCache.get(elementType)) {
			$.ajax({
				url: 'GridElementTmpl',
				data : {type: elementType},
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
		link: function (scope, iElement, tAttrs, transclude) {
			var item = scope.$parent.item;
			var type = item.Type;
			
			var sablona = template(type);
			var compiled = $compile(sablona);
			iElement.html(compiled	(scope.$parent));
		}
	};


	return directiveDefinitionObject;
});

appName = function() {
};

function GridListController($scope, $http,  $rootScope, appSettings, $gridApi) {
	var self = this;
	$scope.data = { };
	$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/grids' })
		.success(function(data, status, headers, config) {
			$scope.data = data;
			//console.log(data, "GridListController");
		});
}

function GridPageCtrl($scope, $http, $routeParams, appSettings) {
	$scope.data = {};
	var self = this;

	$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/GetGrid/' + $routeParams.Id })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data, "edit");
		})
		
		.error(function (data, status, headers, config) {

		});


		$scope.add = function (item) {
			var newitem = item ? addItem(item) : addNewline();
			console.log(newitem);

			$http({
				method: 'POST',
				url: '/adminApi/' + appSettings.Name + '/AddGridElement',
				data: {
					data: newitem,
					gridId: $scope.data.Id
				}
			}).success(function (data, status, headers, config) {
				console.log(data, "ser");
				if (data.Line >= $scope.data.Lines.length) {
					$scope.data.Lines.push([]);
				}
				$scope.data.Lines[data.Line][data.Position] = data;
			});
			$scope.newName = '';
		};
		$scope.remove = function (item) {
			console.log(item);
			var index = $scope.data.Lines[item.Line].indexOf(item);

			if (index != -1) $scope.data.Lines[item.Line].splice(index, 1);

			$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/DeleteGridElement', data: item })
				.success(function (data, status, headers, config) {
					console.log("deleted");
				});
		};

		$scope.edit = function(item, $element) {
			item.Edit = 1;
		};

		$scope.save = function (item) {
			var data = jQuery.extend(true, {}, item);

			if (angular.isObject(data.Content))
				data.Content = JSON.stringify(data.Content);

			$http({
				method: 'POST',
				url: '/adminApi/' + appSettings.Name + '/UpdateGridElement/' + $scope.data.Id,
				data: data
			}).success(function () {
				item.Edit = 0;

			});

		};


			var addNewline = function () {
				var newlineIndex = $scope.data.Lines.length;
				var newline = [];
				var newitem = { Width: 12, Type: $scope.newName, Line: newlineIndex };

				newline.push(newitem);

				return newitem;
			};
			var addItem = function (newitem) {
				return newitem;
			};
}

function CreateCtrl($scope) {
	
}
function EditCtrl($scope, $route, $routeParams, $location, $http, $log, $templateCache, appSettings) {
	$scope.data = {};
	console.log($routeParams);

	$scope.sablona = "GridElementTmpl/novinka";

	$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/GetGridElement/' + $routeParams.GridElementId })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data, "edit");
		})

		.error(function (data, status, headers, config) {

		});


}

