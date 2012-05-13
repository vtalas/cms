angular.module('cmsapi', ['ngResource']).
    factory('Project', function ($resource) {
    	
		var project = $resource('/adminApi/' + appSettings.Name + '/GetGrid/' + $routeParams.Id,
            { Link: '@link', ApplicationName: 'test1' }, {
            	get: { method: 'POST' }
            }
        );
    	return project;
    }).
    factory('GridApi', function ($resource) {
    	var project = $resource('/adminApi/:applicationName/grids',
    		{applicationName : "test1"}, 
    		{
            	get: { method: 'POST' }
            }
        );
    	return project;
    });
