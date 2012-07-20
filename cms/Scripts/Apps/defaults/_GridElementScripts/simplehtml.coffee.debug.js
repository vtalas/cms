(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var simplehtml;
  simplehtml = function($scope, $http) {
    var converter, toHtml;
    text.$inject = ["$scope", "$http"];
    converter = new Showdown.converter();
    toHtml = function(markdown) {
      if (markdown) {
        return converter.makeHtml(markdown);
      }
    };
    $scope.ContentToHtml = function() {
      return toHtml($scope.gridelement.Content);
    };
    return 1;
  };
  window.simplehtml = simplehtml;
}).call(this);
