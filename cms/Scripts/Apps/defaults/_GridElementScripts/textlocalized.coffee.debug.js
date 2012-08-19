(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var textlocalized;
  textlocalized = function($scope, $http, appSettings) {
    textlocalized.$inject = ["$scope", "$http", "appSettings"];
    $scope.aaa = function() {
      return console.log($scope.gridelement);
    };
    return 1;
  };
  window.textlocalized = textlocalized;
}).call(this);
