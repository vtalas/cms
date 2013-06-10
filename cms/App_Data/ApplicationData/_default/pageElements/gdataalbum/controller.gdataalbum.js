var gdataalbum = function ($scope, gdataPhotos, appSettings) {
	var getAlbums,
		gridelement;
	gdataalbum.$inject = ["$scope", "gdataPhotos", "appSettings"];

	getAlbums = function () {
		gdataPhotos.getAlbums(function (data) {
			$scope.albums = data;
		}, function (error) {
			return $scope.haserror = true;
		});

	};

	gridelement = $scope.getGridElement();

	$scope.showGdataAlbumsValue = false;
	$scope.applicationId = appSettings.Id;

	$scope.$on("ngcClickEdit.showPreview", function (e, data) {
		return $scope.$emit("gridelement-save", gridelement);
	});

	$scope.addAlbum = function (id) {
		gridelement.Content = {
			gdataAlbumId: id,
			updated: new Date().getTime()
		};
		$scope.showGdataAlbumsValue = $scope.hideGdataAlbums();
		$scope.$broadcast("gridelement-save", gridelement.Content);
		gridelement.Content = JSON.stringify(gridelement.Content);
		$scope.$emit("gridelement-save", gridelement);
	};
	$scope.hasAlbum = function () {
		return gridelement.Content !== null;
	};
	$scope.isEditMode = function () {
		return !!gridelement.Edit;
	};
	$scope.showGdataAlbums = function () {
		if ($scope.albums === undefined) {
			getAlbums();
		}
		return $scope.showGdataAlbumsValue = true;
	};
	$scope.toggleGdataAlbums = function () {
		if ($scope.albums === undefined && !$scope.showGdataAlbumsValue) {
			getAlbums();
		}
		return $scope.showGdataAlbumsValue = !$scope.showGdataAlbumsValue;
	};
	$scope.hideGdataAlbums = function () {
		return $scope.showGdataAlbumsValue = false;
	};
	$scope.isShowGdataAlbums = function () {
		return !!gridelement.Edit && $scope.showGdataAlbumsValue;
	};
	return 1;
};
