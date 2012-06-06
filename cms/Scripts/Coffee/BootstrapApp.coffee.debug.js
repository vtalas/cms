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
  module.directive("bootstrapelem", [
    function() {
      var directiveDefinitionObject;
      directiveDefinitionObject = {
        link: function(scope, el, tAttrs, controller) {
          scope.$watch(tAttrs.ngModel, function() {
            var val;
            val = el.val();
            if (val[0] === "#") {
              el.css("background", val);
            }
            return console.log(val[0]);
          });
          return 1;
        }
      };
      return directiveDefinitionObject;
    }
  ]);
  aaaController = function($scope, $http) {};
}).call(this);
