var module = angular.module("gridsmodule", ['cmsapi', "templateExt"]);

module.config(['$routeProvider', '$provide', function ($routeProvider, $provide) {
	$provide.factory('appSettings', function () {
		var e = angular.element(".gridsmodule");

		var settings = {
			Name: e.data("application-name"),
			Id: e.data("application-id"),
			Culture: e.data("application-culture")
		};
		return settings;
	});
    $provide.constant();
	$routeProvider
		.when('/gridpages', { controller: GridListController, templateUrl: 'template/gridpages' })
		.when('/menus', { controller: menusCtrl, templateUrl: 'template/menus' })
		.when('/menu/:Id', { controller: menuCtrl, templateUrl: 'template/menu' })
		.when('/gridpage/:Id', { controller: GridPageCtrl, templateUrl: 'template/gridpage' })
		.otherwise({ redirectTo: '/gridpages' });
} ]);

module.controller("cultureCtrl", function ($scope, appSettings, GridApi) {
	$scope.currentCulture = appSettings.Culture;
	$scope.setCulture = function (culture) {

		if (culture != appSettings.Culture) {
			GridApi.setCulture({
				culture: culture
			}, function () {
				appSettings.Culture = culture;
				$scope.currentCulture = culture;
				$scope.$parent.$root.$broadcast("setCultureEvent");
			});
		}
	};
});

//module.directive("gridelement", ['$compile', "GridApi", "appSettings","$http",
module.directive("gridelement", function ($compile, GridApi, appSettings, gridtemplate) {

	function _newitem(line) {
		var newitem = { Id: 0, Width: 12, Type: "text", Line: line, Edit: 0 };
		return newitem;
	}

	var GridElementCtrl = function ($scope) {
		$scope.add = function (item) {
			GridApi.AddGridElement({
				applicationId: appSettings.Id,
				data: item,
				gridId: $scope.grid.Id
			}, function (data) {
				if (data.Line >= $scope.grid.Lines.length - 1) {
					var newitem = _newitem($scope.grid.Lines.length);
					$scope.grid.Lines.push([newitem]);
				}
				$scope.grid.Lines[data.Line][data.Position] = data;
				//TODO: nevyvola se broadcast
				$scope.edit(data);

			});
		};

		$scope.remove = function (item) {
			var line = $scope.$parent.$parent.line;

			GridApi.DeleteGridElement({ applicationId: appSettings.Id, data: item, gridId: $scope.grid.Id },
				function () {
					item.Id = 0; item.Edit = 0; item.Content = "";
					//refresh - preopocitani poradi radku
					if (line.length == 1) {
						$scope.$emit("refreshgrid");
					}
				});
		};

		$scope.edit = function (item) {
			$scope.$broadcast("gridelement-edit");
            item.Edit = 1;
		};

		$scope.save = function (item) {
			var copy = jQuery.extend(true, {}, item);

			if (angular.isObject(copy.Content))
				copy.Content = JSON.stringify(copy.Content);

			GridApi.UpdateGridElement({ applicationId: appSettings.Id, data: copy },
				function () {
                    item.Edit = 0;
                });
		};

	};

	var directiveDefinitionObject = {
		scope: { grid: "=", gridelement: "=" },
		controller: GridElementCtrl,
		link: function (scope, iElement, tAttrs, controller) {
			scope.gui = {edit:0};
            var sablona = gridtemplate(scope.gridelement.Type);
			var compiled = $compile(sablona)(scope);
			iElement.html(compiled);
		}
	};
	return directiveDefinitionObject;
});

