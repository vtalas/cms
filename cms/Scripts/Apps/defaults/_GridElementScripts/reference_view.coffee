###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

reference = ($scope, $http, appSettings) ->
#  reference.$inject = [ "$scope", "$http", "GridApi", "appSettings"]

  $scope.grids = []
  $scope.app = appSettings
  $scope.Link = $scope.gridelement.Resources[0].Value


  console.log($scope.gridelement, "sdsd ref")


#  $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) or { Id: null, Link : null}

#  if $scope.gridelement.Content.Id
#    GridApi.getGrid({Id:$scope.gridelement.Content.Id}, (data)->
#      console.log($scope.gridelement)
#      $scope.destination = data
#    )
  1

window.reference = reference
