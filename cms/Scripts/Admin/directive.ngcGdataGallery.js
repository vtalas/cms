function ngcGDataGallery(gdataPhotos) {
	function dateReadable(date) {
		var current = new Date();
		console.log(current - date);
		if (current - date < 60000 ) {
			return "pod minutu";
		}
		if (current - date < 1200000 ) {
			return (current - date) / 1000 / 60 + " min";
		}
		var curr_date = date.getDate();
		var curr_month = date.getMonth() + 1; //Months are zero based
		var curr_year = date.getFullYear();
		return (curr_date + "-" + curr_month + "-" + curr_year);
	}

	return {
		scope: {
			ngcGdataGallery: "=",
			itemsCount: "="
		},
		link: function (scope, element, attr) {
			var g = scope.ngcGdataGallery || {},
				id = g.gdataAlbumId || null,
				updated = g.updated || null;

			scope.showAllValue = false;

			var getCurrentAlbum = function (id, clearCache, callback) {
				if (id !== null) {
					gdataPhotos.getAlbumPhotos({id: id, refreshCache: clearCache }, function (data) {
						var d = dateReadable(new Date(updated));
						scope.albumInfo = '<div style="text-align:left"><div><b>Polsko</b></div><div>aktualizováno: '+ d +'</div><div>fotek: '+ data.length+'</div></div>';
						scope.albumPhotosAll = data.slice();
						scope.firstPhoto = data.splice(0, 1)[0];
						scope.albumPhotos = data;
						scope.updated = updated;
						scope.length = data.length;
						if (typeof callback === "function") {
							callback();
						}
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
				var updated = new Date().getTime(),
					content = {
						gdataAlbumId: id,
						updated: updated
					};
				scope.$emit("gdataalbum-refresh", content);
				getCurrentAlbum(id, true, function () {
					scope.updated = updated;
				});
			};

			scope.toggleGdataAlbums = function () {
				scope.$parent.toggleGdataAlbums();
			};

			setTimeout(function () {
				getCurrentAlbum(id, false);
			}, 1000);

			scope.$on("gdata-gallery-save", function (e, data) {
				getCurrentAlbum(data.gdataAlbumId);
			});
		}
	};
}