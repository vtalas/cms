var gdataalbum = function ($scope, gdataPhotos, appSettings) {
	var getAlbums,
		gridelement;
	gdataalbum.$inject = ["$scope", "gdataPhotos", "appSettings"];

	$scope.groups = ["aaa", "bb", "ccc"]
	getAlbums = function () {
		gdataPhotos.getAlbums(function (data) {
			console.log("xxxx")
			$scope.albums = data;
		}, function (error) {
			return $scope.haserror = true;
		});
	};

	gridelement = $scope.getGridElement();


	$scope.showGdataAlbumsValue = false;
	$scope.applicationId = appSettings.Id;

	$scope.addAlbum = function (id, name) {
		gridelement.Content = {
			gdataAlbumId: id,
			name: name,
			updated: new Date().getTime()
		};
		$scope.showGdataAlbumsValue = $scope.hideGdataAlbums();
		$scope.$broadcast("gdata-gallery-save", gridelement.Content);
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

	$scope.$on("ngcClickEdit.showPreview", function (e, data) {
		return $scope.$emit("gridelement-save", gridelement);
	});

	$scope.$on("gdataalbum-refresh", function (e, data) {
	    gridelement.Content = data;
	    $scope.$emit("gridelement-save", gridelement);
	});
};
