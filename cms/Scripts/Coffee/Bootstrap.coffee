###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

bootstrap = ($scope, $http, $element,colorsonly, $filter) ->

  $scope.data = $.parseJSON $element.data("model")

  $scope.colorsrefonly = colorsonly
  console.log colorsonly,"xxx",$scope

  $scope.ppp= ($event,name)->
    all = $scope.data
    item = all[name]
    basiccolors=$filter("nameType")(all, "basiccolor", item.value )
    el = $event.currentTarget

    console.log(basiccolors, $(el).val())
    $(el).typeahead({
                  source:basiccolors,
                  updater: (val)->
                    $(el).val(val)
                    $scope.data[name].value = val
                  items:11
                  });
    1

  $scope.aaa = ($event,item)->
    colorPicker($event)
    colorPicker.exportColor = ->
      item.value = "#"+colorPicker.CP.hex


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
