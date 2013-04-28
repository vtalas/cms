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

		this.cmsApi.getPage({id:link}, function (data) {
			deferred.resolve(data);
		});
		return deferred;
	};

	ApiWrapper.prototype.getPages = function () {
		var self = this,
			deferred = $.Deferred();

		this.cmsApi.getPages(function (data) {
			deferred.resolve(data);
		});
		return deferred;
	};


	return ApiWrapper;
}());