var Picus = (function(){

	function Picus(jsonData, repository) {
		this.data = jsonData;
		this.repo = repository;
	}

	Picus.prototype.addGrid  = function (newitem) {
		newitem.id = "grid_" + (this.data.length + 1);
		this.data.push(newitem);
		this.repo.set(this.data);
	};

	Picus.prototype.save  = function () {
		this.repo.set(this.data);
	};

	Picus.prototype.update  = function (item) {
		var index = this.data.indexOf(item);
		this.data[index] = item;
		this.repo.set(this.data);
	};

	Picus.prototype.remove  = function (item) {
		this.data.splice(this.data.indexOf(item), 1);
		this.repo.set(this.data);
	};

	Picus.prototype.getGrid  = function (gridId) {
		for (var i = 0; i < this.data.length; i++) {
			var obj = this.data[i];
			if (obj.id === gridId ) {
				obj.GridElements = obj.GridElements || [];
				return obj;
			}
		}
		return null;
	};

	return Picus;
}());

angular.module('cmsapi', ['ngResource', 'appSettingsModule'])
	.factory('GridApixx', ['$resource', 'appSettings', "$json" , function ($resource, appSettings, $json) {
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
				save: { method: 'PUT', params: { action: "PutData" } }
			}
		);
		return project;
	}])
	.factory('GridApi', ['$resource', 'appSettings', "$json" , function ($resource, appSettings, $json) {
		var project = {};

		project.load = function () {
			return $json.get();
		};
		project.save = function (data, success) {
			$json.set(data).success(function () {
				if (typeof success === "function") {
					success();
				}
			});
		};
		return project;
	}])
	.factory("gridEelementApi", ['$resource', 'appSettings', function ($resource, appSettings) {
		var project = $resource('/api/:applicationId/:controller/:action',
			{
				applicationId: appSettings.Id,
				controller: "adminapi"
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
				put: { method: 'PUT', params: { action: "putData" } },

				//save: { method: 'POST' },
				//query: { method: 'GET', isArray: true },
				//remove: { method: 'DELETE' },
				//delete: { method: 'DELETE' }
			}
		);

		return project;
	}])
	.factory("gdataPhotos", ['$resource', 'appSettings', function ($resource, appSettings) {
		var project = $resource('/api/:applicationId/:controller/:action/:Id',
			{
				applicationId: appSettings.Id,
				controller: "GdataPhotos"
			},
			{
				getAlbums: { method: 'GET', isArray: false, params: { action: "getAlbums" } },
				getAlbum: { method: 'GET', isArray: false, params: {action: "getAlbum"} },
				getAlbumPhotos: { method: 'GET', isArray: true, params: {action: "GetAlbumPhotos"} }
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
	}])
	.factory('cmsUsersApi', ['$resource', 'appSettings', function ($resource, appSettings) {

		return $resource('/api/:applicationId/:controller/:action/:Id',
			{ applicationId: appSettings.Id, controller: "CmsUsersApi" },
			{
				getUsers: { method: 'GET', isArray: true, params: { action: "GetUsers" } },
				postUser: { method: 'POST', isArray: false, params: { action: "PostUser" } },
				putUser: { method: 'PUT', isArray: false, params: { action: "PutUser" } },
				getUserData: { method: 'GET', isArray: true, params: { action: "GetUserData" } }
			}
		);
	}])
	.factory('cmsUsersApi', ['$resource', 'appSettings', function ($resource, appSettings) {

		return $resource('/api/:applicationId/:controller/:action/:Id',
			{ applicationId: appSettings.Id, controller: "CmsUsersApi" },
			{
				getUsers: { method: 'GET', isArray: true, params: { action: "GetUsers" } },
				postUser: { method: 'POST', isArray: false, params: { action: "PostUser" } },
				putUser: { method: 'PUT', isArray: false, params: { action: "PutUser" } },
				getUserData: { method: 'GET', isArray: true, params: { action: "GetUserData" } }
			}
		);
	}])
	.factory('settingsApi', ['$resource', 'appSettings', function ($resource, appSettings) {
		return $resource('/api/:applicationId/:controller/:action/:Id',
			{ applicationId: appSettings.Id, controller: "SettingApi" },
			{
				getPhotosSettings: { method: 'GET', isArray: true, params: { action: "GetPhotosSettings" } },
				putPhotosSettings: { method: 'PUT', isArray: true, params: { action: "PutPhotosSettings" } }
			}
		);
	}]);
;
