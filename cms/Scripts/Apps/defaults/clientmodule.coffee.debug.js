(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var module;
  module = angular.module("clientModule", ['ngResource', 'templateExt']);
  module.run(function() {
    return 1;
  });
  module.config(function($provide) {
    $provide.factory("appSettings", function() {
      return {
        applicationId: "7683508e-0941-4561-b9a3-c7df85791d23"
      };
    });
    $provide.factory("clientApi", function($resource, appSettings) {
      var actions, defaults, proj;
      defaults = {
        applicationId: appSettings.applicationId
      };
      actions = {
        getJson: {
          method: 'GET',
          isArray: false,
          params: {
            action: "grids"
          }
        }
      };
      proj = $resource("http://localhost\\:62728/client/json/:applicationId/:link?callback=JSON_CALLBACK", defaults, actions);
      return proj;
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
}).call(this);
