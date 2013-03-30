angular.module('cmsapi', ['ngResource'])
	.factory('GridApi', ['$resource', 'appSettings', function ($resource, appSettings) {
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
			}
		);

		project.prototype.save = function (cb) {
			return project.save({ id: this._id.$oid },
				angular.extend({ }, this, { _id: undefined }), cb);
		};
		project.prototype.getGrid = function (cb) {
			return project.getGrid({ id: this._id.$oid },
				angular.extend({ }, this, { _id: undefined }), cb);
		};
		return project;
	}])

	.factory("gridEelement", ['$resource', 'appSettings', function ($resource, appSettings) {
		var project = $resource('/api/:applicationId/:controller/:action/:Id',
			{
				applicationId: appSettings.Id,
				controller: "gridElementApi"
			},
			{
//				AddGridElement: { method: 'POST', params: { action: "PostGridElement" } },
//				DeleteGridElement: { method: 'POST', params: { action: "DeleteGridElement" } },
//				UpdateGridElement: { method: 'POST', params: { action: "UpdateGridElement" } }
			}
		);
		return project;
	}])
	.factory('apimenu', ['$resource', 'appSettings', function ($resource, appSettings) {

		return $resource('/api/:applicationId/:controller/:action/:Id',
			{ applicationId: appSettings.Id, controller: "menu" },
			{
				list: { method: 'GET', isArray: true, params: { action: "list" } },
				get: { method: 'GET', isArray: false, params: { action: "get" } }
			}
		);
	}]);