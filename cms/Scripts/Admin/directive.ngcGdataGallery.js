function ngcGDataGallery(gdataPhotos) {

	function dateReadable(date) {
		var current = new Date(),
			spanMinutes = (current - date) / 1000 / 60,
			spanHours = 0;

		if (spanMinutes < 1 ) {
			return "před chvilkou";
		}
		if (spanMinutes < 60 ) {
			return Math.floor(spanMinutes) + " min.";
		}

		spanHours = spanMinutes / 60;

		if (spanHours < 2 ) {
			return Math.floor(spanHours) + " hodina";
		}
		if (spanHours < 6 ) {
			return Math.floor(spanHours) + " hodiny";
		}
		if (spanHours < 24 ) {
			return Math.floor(spanHours) + " hodin";
		}
		if (spanHours < 48 ) {
			return Math.floor(spanHours / 24) + " den";
		}
		if (spanHours < 144 ) {
			return Math.floor(spanHours / 24) + " dny";
		}
		if (spanHours < 168 ) {
			return Math.floor(spanHours / 24) + " dní";
		}
		if (spanHours > 168 ) {
			return "více než týden."
		}
		return current;
	}

	function GdataGallery(data) {
		this.gdataAlbumId = data.gdataAlbumId || null;
		this.name = data.name || "";
		this.updated = data.updated || null;
	}
	return {
		scope: {
			ngcGdataGallery: "=",
			itemsCount: "="
		},
		
		link: function (scope, element, attr) {
			scope.loading = false;
			scope.gallery = new GdataGallery(scope.ngcGdataGallery || {});

			scope.$watch("ngcGdataGallery", function (newValue) {
				scope.gallery = new GdataGallery(newValue || {});
		    });
		    scope.showAllValue = false;

			var getCurrentAlbum = function (albumId, clearCache, callback) {
				if (albumId !== null) {
					scope.loading = true;
					gdataPhotos.getAlbumPhotos({ id: albumId, refreshCache: clearCache }, function (data) {
				        scope.albumInfo = '<div style="text-align:left"><div><b>' + scope.gallery.name + '</b></div><div>fotek: ' + data.length + '</div></div>';

				        scope.length = data.length;
				        scope.albumPhotosAll = data.slice();
						scope.firstPhoto = data.splice(0, 1)[0];
						scope.albumPhotos = data;
						scope.updated = scope.gallery.updated;
						scope.updatedTimeSpan = dateReadable(new Date(scope.gallery.updated));

						if (typeof callback === "function") {
							callback();
						}
						scope.loading = false;
					});
				} else {
					scope.albumPhotosAll = [];
				}
			};

			scope.showAll = function () {
				scope.showAllValue = true;
			};

			scope.hide = function () {
				scope.showAllValue = false;
			};

			scope.refreshAlbums = function () {
				scope.albumPhotosAll = null;
				var updatedDate = new Date().getTime();

				scope.gallery.updated = updatedDate;
				scope.$emit("gdataalbum-refresh", JSON.stringify(scope.gallery));
				getCurrentAlbum(scope.gallery.gdataAlbumId, true, function () {
					scope.updated = updatedDate;
				});
			};

			scope.toggleGdataAlbums = function () {
			    scope.$parent.toggleGdataAlbums();
			};

			getCurrentAlbum(scope.gallery.gdataAlbumId, false);

			scope.$on("gdata-gallery-save", function (e, data) {
				getCurrentAlbum(data.gdataAlbumId);
			});

			scope.$on("gridpage-tick", function () {
				scope.updatedTimeSpan = dateReadable(new Date(scope.gallery.updated));
				scope.$digest();
			});

		}
	};
}