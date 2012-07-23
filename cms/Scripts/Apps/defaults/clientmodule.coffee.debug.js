(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var appController, galleryCtrl, linkCtrl, module;
  module = angular.module("clientModule", ['ngResource', 'templateExt']);
  module.run(function() {
    return 1;
  });
  module.config(function($provide, $routeProvider) {
    $provide.factory("appSettings", function() {
      var setings;
      return setings = {
        applicationId: "7683508e-0941-4561-b9a3-c7df85791d23",
        serverUrl: "http://localhost\\:62728"
      };
    });
    $provide.factory("clientApi", function($resource, appSettings) {
      var actions, defaults, proj;
      defaults = {
        applicationId: appSettings.applicationId
      };
      actions = {
        gridpageJson: {
          method: 'GET',
          isArray: false,
          params: {
            action: "grids"
          }
        }
      };
      proj = $resource(appSettings.serverUrl + "/client/json/:applicationId/:link?callback=JSON_CALLBACK", defaults, actions);
      return proj;
    });
    $routeProvider.when('/link/:link', {
      controller: linkCtrl,
      templateUrl: 'link-template'
    }).when('/gallery/:link', {
      controller: galleryCtrl,
      templateUrl: 'link-template'
    });
    return 1;
  });
  module.directive("gridelement", function(gridtemplate, gridtemplateClient, $compile, $templateCache) {
    var directiveDefinitionObject;
    directiveDefinitionObject = {
      scope: {
        grid: "=",
        gridelement: "="
      },
      link: function(scope, iElement, tAttrs, controller) {
        var compiled, sablona, type;
        type = scope.gridelement.Type;
        sablona = $("#" + type + "-template").html();
        compiled = $compile(sablona)(scope);
        return iElement.html(compiled);
      }
    };
    return directiveDefinitionObject;
  });
  appController = function($scope, $routeParams, clientApi) {
    $scope.thumbs = [];
    return $scope.refresh = function(link) {
      return $scope.thumbs = [link, "asdasd", "jhasvdjs", "jhasvdjd"];
    };
  };
  window.appController = appController;
  linkCtrl = function($scope, $routeParams, clientApi) {
    var p;
    p = $routeParams;
    return clientApi.gridpageJson({
      link: p.link
    }, function(data) {
      return $scope.data = data;
    });
  };
  window.linkCtrl = linkCtrl;
  galleryCtrl = function($scope, $routeParams, clientApi) {
    var p;
    p = $routeParams;
    $scope.$parent.refresh(p.link);
    return clientApi.gridpageJson({
      link: p.link
    }, function(data) {
      return $scope.data = data;
    });
  };
  window.linkCtrl = linkCtrl;
}).call(this);
