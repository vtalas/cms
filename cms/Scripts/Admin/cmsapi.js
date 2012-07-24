angular.module('cmsapi', ['ngResource'])
	.factory('GridApi', [ '$resource',function ($resource, appConfig, $provide) {

        var project = $resource('/adminApi/:applicationId/:action/:Id',
    		{applicationId : "@applicationId"},
            {
                grids: { method: 'GET' ,isArray:true, params: {action : "grids"}},
	            gridpageJson: { method: 'GET' ,isArray:false, params: {action : "grids"}},
                getGrid: { method: 'POST' , params: {action : "GetGrid"}},
                AddGridElement: { method: 'POST' , params: {action : "AddGridElement"}},
                DeleteGridElement: { method: 'POST' , params: {action : "DeleteGridElement"}},
                UpdateGridElement: { method: 'POST' , params: {action : "UpdateGridElement"}}
            }
        );
        project.prototype.save = function(cb) {
            return project.save({id: this._id.$oid},
                angular.extend({}, this, {_id:undefined}), cb);
        };
        project.prototype.getGrid = function(cb) {
			return project.getGrid({ id: this._id.$oid },
		        angular.extend({}, this, {_id:undefined}), cb);
        };

    	return project;
    }]);
