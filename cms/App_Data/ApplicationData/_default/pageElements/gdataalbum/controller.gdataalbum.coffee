###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

gdataalbum = ($scope, gdataPhotos) ->
	gdataalbum.$inject = [ "$scope", "gdataPhotos" ]

	getCurrentAlbum = (item) ->
		if (item.Content != null)
			data = JSON.parse(item.Content);
			if ($scope.albums)
				currentAlbum = $scope.albums[data.gdataAlbumId]

		currentAlbum

	gridelement = $scope.$parent.gridelement;
	$scope.showGdataAlbumsValue = false;

	gdataPhotos.getAlbums((data) ->
		$scope.albums = data;
		$scope.currentAlbum = getCurrentAlbum(gridelement)
		1)

	$scope.$on "ngcClickEdit-showPreview", (e, data) ->
		$scope.$emit("gridelement-save", gridelement);

	$scope.addAlbum = (id) ->

		gridelement.Content = JSON.stringify({ gdataAlbumId:  id});
		$scope.currentAlbum = getCurrentAlbum(gridelement);

		$scope.showGdataAlbumsValue = $scope.hideGdataAlbums();
		$scope.$emit("gridelement-save", gridelement);
		$scope.$apply;

	$scope.hasAlbum = () ->
		return gridelement.Content != null

	$scope.isEditMode = () ->
		return !!gridelement.Edit;

	$scope.showGdataAlbums = () ->
		$scope.showGdataAlbumsValue = true;

	$scope.hideGdataAlbums = () ->
		$scope.showGdataAlbumsValue = false;


	$scope.isShowGdataAlbums = () ->
		return !!gridelement.Edit && $scope.showGdataAlbumsValue;

	1

window.gdataalbum = gdataalbum