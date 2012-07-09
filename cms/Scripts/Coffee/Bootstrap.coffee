###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

xxx = ($scope) ->
  $scope.data = {"aaa":"xxx"}

1
window.xxx = xxx

bootstrap = ($scope, $http, $element,colorsonly) ->

  $scope.data = $.parseJSON $element.data("model")

  $scope.colorsrefonly = colorsonly

  $scope.showColorPicker = ($event,item)->
    colorPicker($event)
    colorPicker.exportColor = ->
      item.value = "#"+colorPicker.CP.hex

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
