
/*
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js


*/


(function() {
  var novinka;

  novinka = function($scope, appSettings) {
    var converter, toHtml;
    $scope.item = $scope.$parent.item;
    $scope.item.Content = angular.fromJson($scope.item.Content) || {
      header: "xxx"
    };
    ({
      text: "xxxccccc"
    });
    converter = new Showdown.converter();
    toHtml = function(markdown) {
      return converter.makeHtml(markdown);
    };
    $scope.headerHtml = function() {
      return toHtml($scope.item.Content.header);
    };
    $scope.textHtml = function() {
      return toHtml($scope.item.Content.text);
    };
    return $scope.thumb = function() {
      return $scope.item.thumb;
    };
  };

  novinka.$inject = ["$scope", "appSettings"];

}).call(this);
