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
            	get: { method: 'POST' ,isArray:true}
            }
        );
        project.prototype.save = function(cb) {
            console.log("save");
            return project.save({id: this._id.$oid},
                angular.extend({}, this, {_id:undefined}), cb);
        };

    	return project;
    });
