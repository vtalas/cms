var module = angular.module("appSettingsModule", ["cmsapi"]);

module.config(['$provide', function($provide) {
    $provide.factory('appSettings', function() {
        //var e = angular.element(".gridsmodule"),
        var e = $(".gridsmodule");
		return {
			Name: e.data("application-name"),
			Id: e.data("application-id"),
			Culture: e.data("application-culture")
		};
    });
}]);