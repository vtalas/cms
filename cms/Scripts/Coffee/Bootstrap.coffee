###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

bootstrap = ($scope, $http, $element,colorsrefonly) ->

  $scope.data = $.parseJSON $element.data("model")

  $scope.colorsrefonly = colorsrefonly
  console.log colorsrefonly,"xxx",$scope

  #console.log $scope.data

  $scope.hider = []

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
