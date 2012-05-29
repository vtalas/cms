###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

novinka = ($scope, appSettings) ->
  $scope.item = $scope.$parent.item
  $scope.item.Content = angular.fromJson($scope.item.Content) or
    header: ""
    text: ""

  converter = new Showdown.converter()
  toHtml = (markdown) ->
    converter.makeHtml markdown

  $scope.headerHtml = ->
    toHtml $scope.item.Content.header

  $scope.textHtml = ->
    toHtml $scope.item.Content.text

  $scope.thumb = ->
    $scope.item.thumb

novinka.$inject = [ "$scope", "appSettings" ]