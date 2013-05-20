var module = angular.module("templateExt", [])
	.factory("gridtemplate", ["$templateCache", function ($templateCache) {

		return function (type) {
			var elementType;
			elementType = type != null ? type : {
				type: "text"
			};
			return $templateCache.get(elementType);
		};
	}]);
//
//module.factory.factory("menuItemTemplate", ["$templateCache", function ($templateCache) {
//	return function (type) {
//		var elementType;
//
//		elementType = type != null ? type : {
//			type: "text"
//		};
//		return $templateCache.get(elementType);
//	};
//}]);
//
//module.factory.factory("gridtemplateClient", function () {
//	return function (type) {
//		var elementType;
//
//		elementType = type != null ? type : {
//			type: "text"
//		};
//		return $.ajax({
//			url: '/Templates/GridElementTmplClient',
//			data: {
//				type: elementType
//			},
//			async: false
//		}).responseText;
//	};
//});

