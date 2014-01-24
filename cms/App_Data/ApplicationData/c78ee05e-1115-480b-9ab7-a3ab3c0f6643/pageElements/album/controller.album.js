var gdataalbum2 = function ($scope, gdataPhotos, appSettings, $markdown) {
	var getAlbums,
		gridelement;
	gdataalbum.$inject = ["$scope", "gdataPhotos", "appSettings", "$markdown"];

	$scope.groups = [];
	$scope.thumbnailRatios = [
		{ name: "3:4", val: "ratio3_4"  },
		{ name: "16:9", val: "ratio16_9" }
	];

	getAlbums = function () {
		gdataPhotos.getAlbums(function (data) {
			$scope.albums = data;
		}, function (error) {
			return $scope.haserror = true;
		});
	};
	gridelement = $scope.getGridElement();
	gridelement.Content.ratio = gridelement.Content.ratio || $scope.thumbnailRatios[0].val;

	$scope.toHtml = function (value) {
		return $markdown.toHtml(value);
	};
	$scope.showGdataAlbumsValue = false;
	$scope.applicationId = appSettings.Id;
	$scope.groups = $scope.getGroups();

	$scope.saveGridElement = function() {
		$scope.$emit("gridelement-save", gridelement);
	};

	$scope.addAlbum = function (item) {
		var id = item.Id,
			name = item.Link,
			thumbnail = item.Thumbnail.Thumbnails[2].PhotoUri;

		var album = {
			gdataAlbumId: id,
			name: name,
			thumbnail: thumbnail,
			updated: new Date().getTime()
		};
		angular.extend(gridelement.Content, album);
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
		angular.extend(gridelement.Content, data);
	    $scope.$emit("gridelement-save", gridelement);
	});
};
