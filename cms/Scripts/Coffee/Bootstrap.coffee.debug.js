(function() {
  /*
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var bootstrap;
  bootstrap = function($scope, $http) {
    console.log("kjabsdjkasd");
    $scope.xxx = "bootstrap";
    return $http({
      method: "POST",
      url: "/bootstrap/Current"
    }).success(function(data, status, headers, config) {
      return console.log(data);
    });
  };
  window.bootstrap = bootstrap;
}).call(this);
