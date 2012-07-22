// Generated by CoffeeScript 1.3.3

/*
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
*/


(function() {
  var reference;

  reference = function($scope, $http, GridApi, appSettings) {
    reference.$inject = ["$scope", "$http"];
    $scope.$on("gridelement-edit", function() {
      if ($scope.grids.length === 0) {
        console.log("load..");
        return $scope.grids();
      }
    });
    $scope.grids = [];
    $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) || {
      Id: ""
    };
    GridApi.getGrid({
      applicationId: appSettings.Id,
      Id: $scope.gridelement.Content.Id
    }, function(data) {
      return $scope.destination = data;
    });
    $scope.choose = function(grid) {
      $scope.destination = grid;
      $scope.gridelement.Content.Id = grid.Id;
      return $scope.$parent.Edit = 0;
    };
    $scope.grids = function() {
      return GridApi.grids({
        applicationId: appSettings.Id
      }, function(data) {
        console.log(data);
        return $scope.grids = data;
      });
    };
    return 1;
  };

  window.reference = reference;

}).call(this);
