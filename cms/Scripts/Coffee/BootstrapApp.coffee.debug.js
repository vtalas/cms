(function() {
  /*
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var aaaController, module;
  module = angular.module("bootstrapApp", []);
  module.config(function($routeProvider, $provide, $filterProvider) {
    $filterProvider.register('nameType', function() {
      return function(data, type, name) {
        var key, prop, x;
        x = {};
        console.log(name.substr(1));
        for (key in data) {
          prop = data[key];
          if (prop.type === type && key.indexOf(name.substr(1)) !== -1) {
            x[key] = prop.value;
          }
        }
        return x;
      };
    });
    $filterProvider.register('typevalue', function() {
      return function(data, type, type2) {
        var key, prop, x;
        x = {};
        for (key in data) {
          prop = data[key];
          if (prop.type === type || prop.type === type2 && prop.value[0] !== "@") {
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
    }).when("/bootswatch", {
      controller: aaaController,
      template: "/Content/bootswatch.html"
    }).otherwise({
      redirectTo: '/csstest'
    });
  });
  module.directive("bootstrapelem", function(datajson, colorsrefonly, colorsonly, $filter) {
    var directiveDefinitionObject;
    directiveDefinitionObject = {
      scope: {
        bootstrapelem: "accessor"
      },
      controller: function($scope) {
        $scope.test = function($event) {
          var a, all, basiccolors;
          all = $scope.$parent.data;
          a = $scope.bootstrapelem();
          basiccolors = $filter("nameType")(all, "basiccolor", a.value);
          if (a.value[0] === "@" && a.value.length === 1) {
            console.log(basiccolors, "is aaa");
          }
          return console.log(basiccolors, a.value.length, a.value[0]);
        };
        return 1;
      },
      link: function(scope, el, tAttrs, controller) {
        var all;
        all = scope.$parent.data;
        return scope.$watch('bootstrapelem().value', function() {
          var a, r;
          a = scope.bootstrapelem();
          switch (a.type) {
            case "color":
            case "basiccolor":
              el.width("80px");
              if (a.value[0] !== "@") {
                el.css("background", a.value);
              }
              if (a.value[0] === "@") {
                colorsonly = $filter("typevalue")(all, "color", "basiccolor");
                r = colorsonly[a.value.substr(1)];
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
