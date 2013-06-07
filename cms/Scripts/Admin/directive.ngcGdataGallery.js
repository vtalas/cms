function ngcGDataGallery(gdataPhotos) {
	var loader = "/Content/loader16.gif";
	return {
		scope: {
			ngcGdataGallery: "="
		},
		link: function (scope) {

			var getCurrentAlbum = function (item) {
				console.log(item);
				if (item != null) {
					gdataPhotos.getAlbum({id: item.gdataAlbumId }, function (data) {
						console.log(data);
						scope.currentAlbum = data;
					});
				}
			};
			var g = scope.ngcGdataGallery;
			getCurrentAlbum(g);
		}
	};
}