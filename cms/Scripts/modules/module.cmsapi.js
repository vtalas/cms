angular.module('cmsapi', ['ngResource', 'appSettingsModule'])
    .factory('GridApi', ['$resource', 'appSettings', function($resource, appSettings) {
        var project = $resource('/api/:applicationId/:controller/:action/:Id',
            {
                applicationId: appSettings.Id,
                controller: "adminApi"
            },
            {
                setCulture: { method: 'PUT', params: { action: "PutCulture" } },
                grids: { method: 'GET', isArray: true, params: { action: "grids" } },
                gridpageJson: { method: 'GET', isArray: false, params: { action: "grids" } },
                getGrid: { method: 'GET', params: { action: "GetGrid" } },
                updateGrid: { method: 'POST', params: { action: "UpdateGrid" } },
            }
        );

        project.prototype.save = function(cb) {
            return project.save({ id: this._id.$oid },
                angular.extend({}, this, { _id: undefined }), cb);
        };
        project.prototype.getGrid = function(cb) {
            return project.getGrid({ id: this._id.$oid },
                angular.extend({}, this, { _id: undefined }), cb);
        };
        return project;
    }])
    .factory("gridEelementApi", ['$resource', 'appSettings', function($resource, appSettings) {
        var project = $resource('/api/:applicationId/:controller/:action',
            {
                applicationId: appSettings.Id,
                controller: "gridElementApi"
            },
            {
                get: { method: 'GET', params: { action: "get" } },
                post: {
                    method: 'POST',
                    params: {
                        action: "postToGrid",
                        gridId: "@gridId"
                    }
                },
                remove: { method: 'DELETE', params: { action: "delete" } },
                put: { method: 'PUT', params: { action: "put" } },

                //save: { method: 'POST' },
			    //query: { method: 'GET', isArray: true },
			    //remove: { method: 'DELETE' },
			    //delete: { method: 'DELETE' }
            }
        );

        return project;
    }])
    .factory("gdataPhotos", ['$resource', 'appSettings', function($resource, appSettings) {
        var project = $resource('/api/:applicationId/:controller/:action/:Id',
            {
                applicationId: appSettings.Id,
                controller: "GdataPhotos"
            },
            {
                getAlbums: { method: 'GET', isArray: false, params: { action: "getAlbums" } },
            }
        );
        return project;
    }])
    .factory('apimenu', ['$resource', 'appSettings', function($resource, appSettings) {

        return $resource('/api/:applicationId/:controller/:action/:Id',
            { applicationId: appSettings.Id, controller: "menu" },
            {
                list: { method: 'GET', isArray: true, params: { action: "list" } },
                get: { method: 'GET', isArray: false, params: { action: "get" } }
            }
        );
    }])
    .factory('cmsUsersApi', ['$resource', 'appSettings', function($resource, appSettings) {

        return $resource('/api/:applicationId/:controller/:action/:Id',
            { applicationId: appSettings.Id, controller: "CmsUsersApi" },
            {
                getUsers: { method: 'GET', isArray: true, params: { action: "GetUsers" } },
                addUser: { method: 'PUT', isArray: false, params: { action: "PostUser" } }
            }
        );
    }]);
