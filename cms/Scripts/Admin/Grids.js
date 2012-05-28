///<reference path="../angular.js"/>
///<reference path="../angular-resource.js"/>
angular.module('testmodule', [])
    .value('version', 'v1.0.1');

var module = angular.module("gridsmodule", ['cmsapi']);

module.config(['$routeProvider', '$provide', function ($routeProvider, $provide) {
	$provide.factory('appSettings', function () {
		var e = angular.element(".gridsmodule"); ;
		var settings = {
			Name: e.data("application-name"),
			Id: e.data("application-id")
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

module.directive("gridelement", ['$templateCache', '$compile', "GridApi", "appSettings", function ($templateCache, $compile, GridApi, appSettings) {
	function _newitem(line) {
		var newitem = { Id: 0, Width: 12, Type: "text", Line: line, Edit: 0 };
		return newitem;
	}

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
		scope: { data: "accessor", grid: "accessor" },

		link: function (scope, iElement, tAttrs, controller) {
			scope.item = scope.data();
			scope.grid = scope.grid();

			var sablona = template(scope.item.Type);

			var compiled = $compile(sablona)(scope);
			iElement.html(compiled);

			scope.add = function (item) {
				GridApi.AddGridElement({
					applicationId: appSettings.Id,
					data: item,
					gridId: scope.grid.Id
				}, function (data) {
					if (data.Line >= scope.grid.Lines.length - 1) {
						var newitem = _newitem(scope.grid.Lines.length);
						scope.grid.Lines.push([newitem]);
					}
					data.Edit = 1;
					scope.grid.Lines[data.Line][data.Position] = data;
				});
			};

			scope.remove = function (item) {
				GridApi.DeleteGridElement({ applicationId: appSettings.Id, data: item },
					function () { item.Id = 0; item.Edit = 0; item.Content = ""; });
			};

			scope.edit = function (item) {
				item.Edit = 1;
			};

			scope.save = function (item) {
				var copy = jQuery.extend(true, {}, item());

				if (angular.isObject(copy.Content))
					copy.Content = JSON.stringify(copy.Content);

				GridApi.UpdateGridElement({ applicationId: appSettings.Id, data: copy },
				function () {
					item().Edit = 0;
				});
			};


		}
	};
	return directiveDefinitionObject;
} ]);


function CreateCtrl($scope) {

}

