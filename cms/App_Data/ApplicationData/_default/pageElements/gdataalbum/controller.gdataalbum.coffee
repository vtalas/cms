###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

gdataalbum = ($scope, gdataPhotos) ->
	gdataalbum.$inject = [ "$scope", "gdataPhotos" ]

	gdataPhotos.getAlbums((data) -> 
		console.log(data)
		$scope.albums = data;
		1
	)
	1

window.gdataalbum = gdataalbum