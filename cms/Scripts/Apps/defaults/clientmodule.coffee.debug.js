﻿(function() {
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
        applicationId: "86199013-5887-4743-89dd-29ddc5bc7df7",
        serverUrl: "http://localhost\\:62728",
        currentGallery: "s",
        currentSubGallery: "ss1"
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
          isArray: false
        }
      };
      proj = $resource(appSettings.serverUrl + "/client/json/:applicationId/:link?callback=JSON_CALLBACK", defaults, actions);
      return proj;
    });
    $provide.factory("GridApi", function($resource, appSettings) {
      var actions, defaults, proj;
      defaults = {
        applicationId: appSettings.applicationId
      };
      actions = {
        getGrid: {
          method: 'GET',
          isArray: false,
          params: {
            action: "GetGrid"
          }
        }
      };
      proj = $resource(appSettings.serverUrl + "/clientapi/:applicationId/:action/:Id?callback=JSON_CALLBACK", defaults, actions);
      return proj;
    });
    $routeProvider.when('/link/:link', {
      controller: linkCtrl,
      templateUrl: 'link-template'
    }).when('/gallery/:link/:xxx', {
      controller: galleryCtrl,
      templateUrl: 'link-template'
    }).otherwise('/gallery/:link/:xxx', {
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
  galleryCtrl = function($scope, $routeParams, clientApi, GridApi, appSettings) {
    var p;
    p = $routeParams;
    if (p.xxx) {
      clientApi.gridpageJson({
        link: p.xxx
      }, function(data) {
        return $scope.data = data;
      });
    }
    if (appSettings.currentgallery === p.link) {
      return;
    }
    appSettings.currentgallery = p.link;
    return clientApi.gridpageJson({
      link: p.link
    }, function(data) {
      return $scope.$parent.refreshGalleryThumbs(data);
    });
  };
  window.galleryCtrl = galleryCtrl;
  appController = function($scope, appSettings, clientApi, $routeParams, $location, $route) {
    console.log($routeParams, $routeParams.link, $location, $route);
    return $scope.refreshGalleryThumbs = function(lines) {
      return $scope.galleryThumbs = lines;
    };
  };
  window.appController = appController;
}).call(this);
