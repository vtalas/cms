(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var novinka;
  novinka = function($scope, $http) {
    var converter, toHtml;
    novinka.$inject = ["$scope", "$http"];
    $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) || {
      header: "",
      text: ""
    };
    converter = new Showdown.converter();
    toHtml = function(markdown) {
      var x;
      x = true;
      if (markdown) {
        return converter.makeHtml(markdown);
      }
    };
    $scope.headerHtml = function() {
      return toHtml($scope.gridelement.Content.header);
    };
    $scope.textHtml = function() {
      return toHtml($scope.gridelement.Content.text);
    };
    return $scope.thumb = function() {
      return $scope.gridelement.thumb;
    };
  };
  window.novinka = novinka;
}).call(this);
