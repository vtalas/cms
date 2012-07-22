###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

reference = ($scope, $http, GridApi,appSettings) ->
  reference.$inject = [ "$scope", "$http" ]

  $scope.$on("gridelement-edit",()->

    if $scope.grids.length==0
      console.log "load.."
      $scope.grids()
  )

  $scope.grids = []
  $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) or { Id: ""}

  GridApi.getGrid({ applicationId: appSettings.Id,Id:$scope.gridelement.Content.Id}, (data)->
    $scope.destination = data
  )

  $scope.choose = (grid)->
    $scope.destination = grid
    $scope.gridelement.Content.Id = grid.Id
    $scope.$parent.Edit = 0

  $scope.grids = ()->
    GridApi.grids({ applicationId: appSettings.Id}, (data)->
      console.log data
      $scope.grids = data
    )

  1

window.reference = reference
