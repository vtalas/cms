###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

reference = ($scope, $http, GridApi, appSettings) ->
#  reference.$inject = [ "$scope", "$http", "GridApi", "appSettings"]

  $scope.grids = []
  $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) or { Id: null, Link : null}

  if $scope.gridelement.Content.Id
    GridApi.getGrid({Id:$scope.gridelement.Content.Id}, (data)->
      $scope.destination = data
    )

  # ADmin only
  # load grid list
  $scope.$on("gridelement-edit",()->
    if $scope.grids.length==0
      $scope.grids()
  )

  $scope.choose = (grid)->
    $scope.destination = grid
    $scope.gridelement.Content.Id = grid.Id;
    $scope.gridelement.Content.Link = grid.Link;

    $scope.gridelement.Resources = []
    $scope.gridelement.Resources.push({Id : grid.Resource.Id})

    $scope.$parent.Edit = 0
    $scope.$parent.save($scope.gridelement)

  $scope.grids = ()->
    GridApi.grids(null, (data)->
      $scope.grids = data
    )

  1

window.reference = reference
