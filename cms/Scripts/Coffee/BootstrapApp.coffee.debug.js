(function() {
  /*
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var aaaController, module;
  module = angular.module("bootstrapApp", []);
  module.config([
    "$routeProvider", "$provide", function($routeProvider, $provide) {
      return $routeProvider.when("/csstest", {
        controller: aaaController,
        template: "/Content/csstest.html"
      }).when("/aaa", {
        controller: aaaController,
        template: "/Content/aaa.html"
      }).when("/bootswatch", {
        controller: aaaController,
        template: "/Content/bootswatch.html"
      });
    }
  ]);
  aaaController = function($scope, $http) {
    return 1;
  };
}).call(this);
