###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

reference = ($scope, $http, GridApi, appSettings) ->
#  reference.$inject = [ "$scope", "$http", "GridApi", "appSettings"]
#  console.log("ref",appSettings)


  $scope.grids = []
  $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) or { Id: null}

  if $scope.gridelement.Content.Id
    GridApi.getGrid({Id:$scope.gridelement.Content.Id}, (data)->
      $scope.$emit("reference-loaded")
      console.log("refffff")
      $scope.destination = data
    )

  #  ADmin only
  $scope.$on("gridelement-edit",()->
    if $scope.grids.length==0
      console.log "load.."
      $scope.grids()
  )

  $scope.choose = (grid)->
    $scope.destination = grid
    $scope.gridelement.Content.Id = grid.Id
    $scope.$parent.Edit = 0
    $scope.$parent.save($scope.gridelement)

  $scope.grids = ()->
    console.log(appSettings, "ref")
    GridApi.grids({ applicationId: appSettings.Id}, (data)->
      console.log data
      $scope.grids = data
    )

  1

window.reference = reference
