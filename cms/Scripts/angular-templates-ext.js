
/*
@reference jquery-1.7.2.js
@reference angular.js
*/


(function() {
  var module;

  module = angular.module("templateExt", []);

  module.config(function($provide) {
    $provide.factory("gridtemplate", function($templateCache) {
      return function(type) {
        var elementType;
        elementType = type != null ? type : {
          type: "text"
        };
        console.log(elementType);
        if (!$templateCache.get(elementType)) {
          $.ajax({
            url: '/Templates/GridElementTmpl',
            data: {
              type: elementType
            },
            async: false,
            success: function(data) {
              return $templateCache.put(elementType, data);
            }
          });
        }
        return $templateCache.get(elementType);
      };
    });
    $provide.factory("menuItemTemplate", function($templateCache) {
      return function(type) {
        var elementType;
        elementType = type != null ? type : {
          type: "text"
        };
        if (!$templateCache.get(elementType)) {
          $.ajax({
            url: '/Templates/menuItemTemplate',
            data: {
              type: elementType
            },
            async: false,
            success: function(data) {
              return $templateCache.put(elementType, data);
            }
          });
        }
        return $templateCache.get(elementType);
      };
    });
    $provide.factory("gridtemplateClient", function($templateCache) {
      return function(type) {
        var elementType;
        elementType = type != null ? type : {
          type: "text"
        };
        return $.ajax({
          url: '/Templates/GridElementTmplClient',
          data: {
            type: elementType
          },
          async: false
        }).responseText;
      };
    });
    return 1;
  });

}).call(this);
