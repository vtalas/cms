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
		console.log(data)
		$scope.albums = data;
		$scope.currentAlbum = getCurrentAlbum(gridelement)
		console.log($scope.currentAlbum)

		1)

	$scope.addAlbum = (id) ->
		gridelement.Content = {}
		gridelement.Content.gdataAlbumId = id
		$scope.showGdataAlbumsValue = $scope.hideGdataAlbums();


	$scope.hasAlbum = () ->
		return $scope.currentAlbum != undefined

	$scope.isEditMode = () ->
		return !!gridelement.Edit;

	$scope.showGdataAlbums = () ->
		$scope.showGdataAlbums = true;

	$scope.hideGdataAlbums = () ->
		$scope.showGdataAlbums = true;

	$scope.isShowGdataAlbums = () ->
		return !!gridelement.Edit && $scope.showGdataAlbumsValue;

	1

window.gdataalbum = gdataalbum