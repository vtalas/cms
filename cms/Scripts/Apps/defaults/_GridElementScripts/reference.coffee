###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

reference = ($scope, $http, GridApi, appSettings) ->
#  reference.$inject = [ "$scope", "$http", "GridApi", "appSettings"]
#  console.log("ref",appSettings)


  $scope.grids = []
  $scope.app = appSettings
  $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) or { Id: null}

  if $scope.gridelement.Content.Id
    GridApi.getGrid({Id:$scope.gridelement.Content.Id}, (data)->
      $scope.destination = data
    )

  #  ADmin only
  $scope.$on("gridelement-edit",()->
    console.log "load.."
    if $scope.grids.length==0
      $scope.grids()
  )

  $scope.choose = (grid)->
    $scope.destination = grid
    $scope.gridelement.Content.Id = grid.Id
    $scope.$parent.Edit = 0
    $scope.$parent.save($scope.gridelement)

  $scope.grids = ()->
    console.log(appSettings, "ref")
    GridApi.grids(null, (data)->
      console.log data
      $scope.grids = data
    )

  1

window.reference = reference
