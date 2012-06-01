###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

bootstrap = ($scope, $http, $element,colorsonly) ->

  $scope.data = $.parseJSON $element.data("model")

  $scope.colorsrefonly = colorsonly
  console.log colorsonly,"xxx",$scope

  #console.log $scope.data

  $scope.aaa = ($event,item)->
    colorPicker($event)
    colorPicker.exportColor = ->
      item.value = "#"+colorPicker.CP.hex
      $scope.$apply()


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
