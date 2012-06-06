###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

bootstrap = ($scope, $http) ->
  #novinka.$inject = [ "$scope", "$http", "appSettings" ]

  $scope.hider = []

  $http(
    method: "POST"
    url: "/bootstrap/Current"
  ).success((data, status, headers, config) ->
      $scope.data = data;
      $scope.prdel = "xhjasvbhj"
      console.log data, "loaded"
  )

  $scope.refresh = ->
    $http(
      method: "POST"
      url: "/bootstrap/Refresh"
      data: $scope.data
    ).success((data, status, headers, config) ->
      console.log data
      $scope.refreshtoken = new Date
    )

  $scope.toggleValue = (item) ->
    return (if $scope.hider[item] then true else false)


  $scope.toggle = (item)->
    $scope.hider[item] = !$scope.toggleValue(item);
    console.log $scope.hider, item


  1
window.bootstrap = bootstrap
