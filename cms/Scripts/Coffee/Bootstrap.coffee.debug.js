(function() {
  /*
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var bootstrap;
  bootstrap = function($scope, $http, $element) {
    $scope.data = $.parseJSON($element.data("model"));
    $scope.hider = [];
    $scope.refresh = function() {
      return $http({
        method: "POST",
        url: "/bootstrap/Refresh",
        data: $scope.data
      }).success(function(data, status, headers, config) {
        console.log(data);
        return $scope.refreshtoken = new Date;
      });
    };
    $scope.toggleValue = function(item) {
      if ($scope.hider[item]) {
        return true;
      } else {
        return false;
      }
    };
    $scope.toggle = function(item) {
      $scope.hider[item] = !$scope.toggleValue(item);
      return console.log($scope.hider, item);
    };
    return 1;
  };
  window.bootstrap = bootstrap;
}).call(this);
