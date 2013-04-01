var gDataPhotos = ['$scope', 'gdataPhotos', 'appSettings', function ($scope, gdataPhotos) {

	gdataPhotos.getAlbums(function(data) {
		console.log(data);
		$scope.albums = data;
	});

}]