angular.module('apiModule', ['ngResource', 'appConfigModule'])
	.factory('cmsApi', ['$resource', 'appConfig', function ($resource, appConfig) {
		var api = $resource(appConfig.baseUrl + appConfig.applicationId +'/:link',
			{  },
			{
				getPage: { method: 'GET' }
			});

		return api;
	}]);

