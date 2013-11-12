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
	Picus.prototype.update  = function (item) {
		var index = this.data.indexOf(item);
		this.data[index] = item;
		this.repo.set(this.data);
	};

	Picus.prototype.remove  = function (item) {
		this.data.splice(this.data.indexOf(item), 1);
		this.repo.set(this.data);
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

		project.getGrid = function (params, success) {
			var x = {"Id": "ab8ee05e-1115-480b-9ab7-a3ab3c0f6643", "Name": "galerie 1", "Home": false, "Category": "Page", "Link": "gallery_1", "GridElements": [
				{"Id": "abe46b42-aa0b-4771-b723-3b6fc34c1da8", "Position": 0, "Content": {"gdataAlbumId": "5886301781916071761", "name": "polsko", "updated": 1375942756606}, "Type": "gdataalbum", "Skin": null, "ParentId": "", "Resources": {"header": {"Id": 24, "Value": "asljkdnasjklnd"}, "text": {"Id": 25, "Value": "aaaa"}}, "Width": 12, "Group": []},
				{"Id": "e5147c9f-a937-46df-b651-e53ded12194d", "Position": 1, "Content": {"gdataAlbumId": "5886301972202877905", "name": "xxx", "updated": 1373883074966}, "Type": "gdataalbum", "Skin": null, "ParentId": "", "Resources": {}, "Width": 12, "Group": []},
				{"Id": "16067e08-e599-4ecc-99e7-2e5ac17ee1b8", "Position": 2, "Content": null, "Type": "text", "Skin": null, "ParentId": "", "Resources": {"text": {"Id": 36, "Value": "asdasd"}}, "Width": 12, "Group": []},
				{"Id": "651130b6-8675-4803-a23e-1b9ab9f8a470", "Position": 3, "Content": {"gdataAlbumId": "5886302191458765489", "name": "home", "updated": 1373883076853}, "Type": "gdataalbum", "Skin": null, "ParentId": "", "Resources": {}, "Width": 12, "Group": []},
				{"Id": "8f511834-84eb-4619-949a-432d584fb450", "Position": 4, "Content": null, "Type": "clientspecific", "Skin": null, "ParentId": "", "Resources": {}, "Width": 12, "Group": []},
				{"Id": "3866dffa-17c2-40d9-a875-f79e44e4a630", "Position": 5, "Content": null, "Type": "simplehtml", "Skin": null, "ParentId": "", "Resources": {"text": {"Id": 37, "Value": "asdasd\n===\nkamsdklansdlnaslknd klasnl nlkas \naslkdnlkasndl as\n\nasdlknasldknasklndk asd"}}, "Width": 12, "Group": []},
				{"Id": "9a7ac656-8934-43f1-b071-38713d2cf378", "Position": 6, "Content": {"gdataAlbumId": "5886301781916071761", "name": "polsko", "updated": 1372349153188}, "Type": "gdataalbum", "Skin": null, "ParentId": "", "Resources": {"header": {"Id": 38, "Value": "sdfoihh ashdfhasd fasdf"}, "text": {"Id": 39, "Value": "asdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasi"}}, "Width": 12, "Group": []},
				{"Id": "90add99e-1ec5-4604-b4a5-26ad7b2e56f5", "Position": 7, "Content": {"gdataAlbumId": "5886302191458765489", "name": "home", "updated": 1372349182508}, "Type": "gdataalbum", "Skin": null, "ParentId": "", "Resources": {"header": {"Id": 41, "Value": "qweajosdbnjasibdibasib"}, "text": {"Id": 42, "Value": "asdasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasi"}}, "Width": 12, "Group": []},
				{"Id": "2dd8701b-42bf-4e36-9687-5c85e633a9d0", "Position": 8, "Content": {"gdataAlbumId": "5886301972202877905", "name": "xxx", "updated": 1372670891794}, "Type": "gdataalbum", "Skin": null, "ParentId": "", "Resources": {"header": {"Id": 43, "Value": "asdasd"}, "text": {"Id": 44, "Value": "asdasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasi"}}, "Width": 12, "Group": []},
				{"Id": "3b2155d0-5ed3-4945-9a2e-42d4597807c6", "Position": 9, "Content": {"gdataAlbumId": "5886302191458765489", "name": "home", "updated": 1372349176276}, "Type": "gdataalbum", "Skin": null, "ParentId": "", "Resources": {"header": {"Id": 45, "Value": "asdasdas"}, "text": {"Id": 46, "Value": "asdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasi"}}, "Width": 12, "Group": []},
				{"Id": "6d2e7a09-5fda-4c74-9655-adde1daf9fb7", "Position": 10, "Content": {"gdataAlbumId": "5886301781916071761", "name": "polsko", "updated": 1372349173332}, "Type": "gdataalbum", "Skin": null, "ParentId": "", "Resources": {"text": {"Id": 40, "Value": "asdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasiasdif bui dsfuiasduifuihasuidfhasi"}, "header": {"Id": 47, "Value": "asdasdasd"}}, "Width": 12, "Group": []}
			], "Authorize": false}
			success(x);
		}
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
