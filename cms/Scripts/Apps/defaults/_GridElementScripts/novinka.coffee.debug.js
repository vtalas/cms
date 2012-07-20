(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var novinka;
  novinka = function($scope) {
    var converter, toHtml;
    novinka.$inject = ["$scope"];
    $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) || {
      header: "",
      text: ""
    };
    console.log($scope.gridelement, $scope.gridelement.Content, "novinka.cooffe");
    converter = new Showdown.converter();
    toHtml = function(markdown) {
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
