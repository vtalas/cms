var module = angular.module("gridsmodule", ['cmsapi', "templateExt"]);

module.config(['$routeProvider', '$provide', function ($routeProvider, $provide) {
	$provide.factory('appSettings', function () {
		var e = angular.element(".gridsmodule"); ;
		var settings = {
			Name: e.data("application-name"),
			Id: e.data("application-id")
		};
		return settings;
	});
	$routeProvider
		.when('/list', { controller: GridListController, templateUrl: 'template/list' })
		.when('/gridpage/:Id', { controller: GridPageCtrl, templateUrl: 'template/gridpage' })
		.when('/gridpage/:Id/edit/:GridElementId', { controller: EditCtrl, template: 'template/edit' })
		.otherwise({ redirectTo: '/list' });
}]);

//module.directive("gridelement", ['$compile', "GridApi", "appSettings","$http",
module.directive("gridelement", function ($compile, GridApi, appSettings, gridtemplate) {

    function _newitem(line) {
		var newitem = { Id: 0, Width: 12, Type: "text", Line: line, Edit: 0 };
		return newitem;
	}


    var GridElementCtrl = function($scope){
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
                data.Edit = 1;
                $scope.grid.Lines[data.Line][data.Position] = data;
            });
        };

        $scope.remove = function (item) {
            GridApi.DeleteGridElement({ applicationId: appSettings.Id, data: item },
                                      function () { item.Id = 0; item.Edit = 0; item.Content = ""; });
        };

        $scope.edit = function (item) {
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

    }

	var directiveDefinitionObject = {
		scope: {  grid: "=", gridelement: "=" },
        controller:GridElementCtrl,
		link: function (scope, iElement, tAttrs, controller) {
			var sablona = gridtemplate(scope.gridelement.Type);
			var compiled = $compile(sablona)(scope);
			iElement.html(compiled);
		}
	};
	return directiveDefinitionObject;
} );


function CreateCtrl($scope) {

}









































