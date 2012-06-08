(function() {
  /*
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var aaaController, module;
  module = angular.module("bootstrapApp", []);
  module.config(function($routeProvider, $provide, $filterProvider) {
    $filterProvider.register('typevalue', function() {
      return function(data, type) {
        var key, prop, x;
        x = {};
        for (key in data) {
          prop = data[key];
          if (prop.type === type && prop.value[0] !== "@") {
            x[key] = prop.value;
          }
        }
        return x;
      };
    });
    $filterProvider.register('refs', function() {
      return function(data, type) {
        var key, prop, x;
        x = {};
        for (key in data) {
          prop = data[key];
          if (prop.type === type && prop.value[0] === "@") {
            x[key] = prop.value;
          }
        }
        return x;
      };
    });
    $provide.factory("datajson", function() {
      var d;
      d = $.parseJSON(angular.element("html").data("modeldata"));
      return d;
    });
    $provide.factory("colorsonly", function($filter, datajson) {
      var a;
      a = $filter("typevalue")(datajson, "color");
      return a;
    });
    $provide.factory("colorsrefonly", function($filter, datajson) {
      var a;
      a = $filter("refs")(datajson, "color");
      return a;
    });
    return $routeProvider.when("/csstest", {
      controller: aaaController,
      template: "/Content/csstest.html"
    }).when("/aaa", {
      controller: aaaController,
      template: "/Content/aaa.html"
    }).when("/bootswatch", {
      controller: aaaController,
      template: "/Content/bootswatch.html"
    }).otherwise({
      redirectTo: '/aaa'
    });
  });
  module.directive("bootstrapelem", function(datajson, colorsrefonly, colorsonly, $filter) {
    var directiveDefinitionObject;
    directiveDefinitionObject = {
      scope: {
        bootstrapelem: "accessor"
      },
      link: function(scope, el, tAttrs, controller) {
        var all;
        all = scope.$parent.data;
        return scope.$watch('bootstrapelem().value', function() {
          var a, ccc, r;
          a = scope.bootstrapelem();
          if (a.type === "color") {
            el.width("80px");
            if (a.value[0] !== "@") {
              el.css("background", a.value);
            }
            ccc = $filter("typevalue")(all, "color");
            if (a.value[0] === "@") {
              r = ccc[a.value.substr(1)];
              if (r) {
                return el.css("background", r);
              }
            }
          }
        });
      }
    };
    return directiveDefinitionObject;
  });
  aaaController = function($scope, $http) {};
  1;
}).call(this);
