﻿(function() {
  /*
  @reference ../showdown.js
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var reference;
  reference = function($scope, $http, GridApi, appSettings) {
    $scope.grids = [];
    $scope.destination = {};
    $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) || {
      Id: null,
      Link: null
    };
    if ($scope.gridelement.Content.Id) {
      GridApi.getGrid({
        Id: $scope.gridelement.Content.Id
      }, function(data) {
        return $scope.destination = data;
      });
    }
    $scope.$on("gridelement-edit", function() {
      if ($scope.grids.length === 0) {
        return $scope.grids();
      }
    });
    $scope.choose = function(grid) {
      $scope.destination = grid;
      $scope.gridelement.Content.Id = grid.Id;
      $scope.gridelement.Content.Link = grid.Link;
      $scope.gridelement.Resources = [];
      $scope.gridelement.Resources.push({
        Id: grid.ResourceDto.Id
      });
      $scope.$parent.Edit = 0;
      return $scope.$parent.save($scope.gridelement);
    };
    $scope.grids = function() {
      return GridApi.grids(null, function(data) {
        return $scope.grids = data;
      });
    };
    return 1;
  };
  window.reference = reference;
}).call(this);