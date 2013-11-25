/*global Showdown, angular*/
var stringUtils = angular.module("stringutils", []);

stringUtils.factory("$markdown", function () {
	var converter = new Showdown.converter();

	return {
		toHtml: function (markdownText) {
			if (markdownText) {
				return converter.makeHtml(markdownText);
			}
			return "";
		}
	};
});
