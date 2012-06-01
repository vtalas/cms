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
        var num, x, _i, _len, _ref;
        console.log("kjbsadksjbd");
        x = [];
        _ref = data.grays;
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
          num = _ref[_i];
          if (num.type === type && num.value[0] !== "@") {
            x[num.name] = num.value;
          }
        }
        return x;
      };
    });
    $filterProvider.register('refs', function() {
      return function(data, type) {
        var num, x, _i, _len, _ref;
        console.log("krefsd");
        x = [];
        _ref = data.scaffolding;
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
          num = _ref[_i];
          if (num.type === type && num.value[0] === "@") {
            x[num.name] = num.value;
          }
        }
        return x;
      };
    });
    $provide.factory("datajson", function($filter) {
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
  module.directive("bootstrapelem", function(datajson, colorsonly) {
    var directiveDefinitionObject;
    directiveDefinitionObject = {
      link: function(scope, el, tAttrs, controller) {
        var all;
        console.log(colorsonly, datajson);
        all = scope.$parent.data;
        scope.$watch(tAttrs.ngModel, function() {
          var a, r;
          a = scope.item;
          if (a.type === "color" && a.value[0] === "#") {
            el.css("background", a.value);
          }
          if (a.type === "color" && a.value[0] === "@") {
            r = colorsonly[a.value.substr(1)];
            if (r) {
              return el.css("background", r);
            }
          }
        });
        return 1;
      }
    };
    return directiveDefinitionObject;
  });
  aaaController = function($scope, $http, colorsrefonly) {
    return $scope.colorsrefonly = colorsrefonly;
  };
  1;
}).call(this);
