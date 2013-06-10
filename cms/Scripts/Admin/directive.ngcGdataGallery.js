function ngcGDataGallery(gdataPhotos) {
	var loader = "/Content/loader16.gif";
	return {
		scope: {
			ngcGdataGallery: "=",
			itemsCount: "="
		},
		link: function (scope) {
			var g = scope.ngcGdataGallery || {},
				id = g.gdataAlbumId || null,
				updated = g.updated || null;

			scope.showAllValue = false;

			var getCurrentAlbum = function (id) {
				if (id !== null) {
					gdataPhotos.getAlbumPhotos({id: id }, function (data) {
						scope.albumPhotosAll = data.slice();
						scope.firstPhoto = data.splice(0, 1)[0];
						scope.albumPhotos = data;
						scope.updated = updated;
						scope.length = data.length;
					});
				}
			};

			scope.showAll = function () {
				scope.showAllValue = true;
			};
			scope.hide = function () {
				scope.showAllValue = false;
			};
			scope.$on("gridelement-save", function (e, data) {
				console.log(data);
				getCurrentAlbum(data.gdataAlbumId);
			});
			getCurrentAlbum(id);
		}
	};
}