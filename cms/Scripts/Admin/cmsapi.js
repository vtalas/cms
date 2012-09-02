angular.module('cmsapi', ['ngResource'])
	.factory('GridApi', ['$resource', 'appSettings', function ($resource, appSettings) {

		var project = $resource('/adminApi/:applicationId/:action/:Id',
			{ applicationId: appSettings.Id },
			{
				setCulture: { method: 'POST', params: { action: "SetCulture" } },
				grids: { method: 'GET', isArray: true, params: { action: "grids" } },
				gridpageJson: { method: 'GET', isArray: false, params: { action: "grids" } },
				getGrid: { method: 'POST', params: { action: "GetGrid" } },
				AddGridElement: { method: 'POST', params: { action: "AddGridElement" } },
				DeleteGridElement: { method: 'POST', params: { action: "DeleteGridElement" } },
				UpdateGridElement: { method: 'POST', params: { action: "UpdateGridElement" } }
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
	.factory('apimenu', ['$resource', 'appSettings', function ($resource, appSettings) {

		var project = $resource('/api/:applicationId/:controller/:action/:Id',
			{ applicationId: appSettings.Id, controller: "menu" },
			{
				list: { method: 'GET', isArray: true, params: { action: "list" } },
				get: { method: 'GET', isArray: false, params: { action: "get" } }
			}
		);

		return project;
	}]);