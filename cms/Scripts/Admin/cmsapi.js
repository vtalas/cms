angular.module('appConfig', [])
    .factory("nasme","test1")

angular.module('cmsapi', ['ngResource', 'appConfig'])
    .factory('Project',  [ '$resource',function ($resource) {
    	
		var project = $resource('/adminApi/' + appSettings.Name + '/GetGrid/' + $routeParams.Id,
            { Link: '@link', ApplicationName: 'test1' }, {
            	get: { method: 'POST' }
            }
        );
    	return project;
    }])
    
	.factory('GridApi', [ '$resource',function ($resource, appConfig, $provide) {

        var project = $resource('/adminApi/:applicationId/grids',
    		{applicationId : "test1"},
    		{
            	get: { method: 'GET' ,isArray:true}
            }
        );
        project.prototype.save = function(cb) {
            console.log("save");
            return project.save({id: this._id.$oid},
                angular.extend({}, this, {_id:undefined}), cb);
        };

    	return project;
    }]);
