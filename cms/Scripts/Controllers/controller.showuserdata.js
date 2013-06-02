var showuserdataCtrl = ['$scope', '$http', '$rootScope', 'appSettings', 'cmsUsersApi',
	function ($scope, $http, $rootScope, appSettings, cmsUsersApi) {

		cmsUsersApi.getUserData(function(data) {
			$scope.data = data;
		});
	}];