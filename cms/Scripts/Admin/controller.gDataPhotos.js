var gDataPhotos = ['$scope', 'gdataPhotos', function ($scope, gdataPhotos) {

	gdataPhotos.getAlbums(function(data) {
		console.log(data);
		$scope.albums = data;
	});

}]