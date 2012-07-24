(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var reference;
  reference = function($scope, $http, GridApi, appSettings) {
    $scope.grids = [];
    $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) || {
      Id: null
    };
    if ($scope.gridelement.Content.Id) {
      GridApi.getGrid({
        Id: $scope.gridelement.Content.Id
      }, function(data) {
        $scope.$emit("reference-loaded");
        console.log("refffff");
        return $scope.destination = data;
      });
    }
    $scope.$on("gridelement-edit", function() {
      console.log("load..");
      if ($scope.grids.length === 0) {
        return $scope.grids();
      }
    });
    $scope.choose = function(grid) {
      $scope.destination = grid;
      $scope.gridelement.Content.Id = grid.Id;
      $scope.$parent.Edit = 0;
      return $scope.$parent.save($scope.gridelement);
    };
    $scope.grids = function() {
      console.log(appSettings, "ref");
      return GridApi.grids(null, function(data) {
        console.log(data);
        return $scope.grids = data;
      });
    };
    return 1;
  };
  window.reference = reference;
}).call(this);
