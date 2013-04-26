/*global MenuItemList, RawDataConverter, GalleryList*/
var ApiWrapper = (function () {

	function ApiWrapper(cmsApiImpl, cache) {
		this.cmsApi = cmsApiImpl;
		this.cache = cache;
		//this.converter = new RawDataConverter();
	}


	ApiWrapper.prototype.getPage = function (link) {
		var self = this,
			deferred = $.Deferred();

		this.cmsApi.getPage( {link:link }, function (data) {
			console.log(data);
			deferred.resolve(data);
		});
		return deferred;
	};


	return ApiWrapper;
}());