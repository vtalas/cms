(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var module;
  module = angular.module("clientModule", ['ngResource']);
  module.run(function() {
    return console.log("run clientModule");
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
}).call(this);
