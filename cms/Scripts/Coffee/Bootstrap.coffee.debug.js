(function() {
  /*
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var bootstrap;
  bootstrap = function($scope, $http) {
    console.log("kjabsdjkasd");
    $http({
      method: "POST",
      url: "/bootstrap/Current"
    }).success(function(data, status, headers, config) {
      $scope.data = data;
      return console.log(data);
    });
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
    return 1;
  };
  window.bootstrap = bootstrap;
}).call(this);
