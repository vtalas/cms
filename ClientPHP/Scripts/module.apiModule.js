angular.module('apiModule', ['ngResource', 'appConfigModule'])
	.factory('cmsApi', ['$resource', 'appConfig', function ($resource, appConfig) {
		var api = $resource(appConfig.baseUrl  +'Service/serverProxy.php?action=:action',
//		var api = $resource(appConfig.baseUrl + appConfig.applicationId +'/:link',
			{   },
			{
				getPage: { method: 'GET', isArray: false, params: {action: "getPage"} },
				getPages: { method: 'GET', isArray: true, params: {action: "getPages"} }
			});

		return api;
	}]);

