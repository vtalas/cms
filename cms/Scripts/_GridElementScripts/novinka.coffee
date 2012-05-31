###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

novinka = ($scope, $http, appSettings) ->
  novinka.$inject = [ "$scope", "$http", "appSettings" ]

  $scope.data = $scope.$parent.item
  $scope.data.Content = angular.fromJson($scope.data.Content) or { header: "",  text: ""}

  converter = new Showdown.converter()
  self = this

  toHtml = (markdown) ->
    x = true;
    converter.makeHtml markdown if markdown

  $scope.headerHtml = ->
    toHtml $scope.data.Content.header

  $scope.textHtml = ->
    toHtml $scope.data.Content.text

  $scope.thumb = ->
    $scope.data.thumb

  $scope.add = ->

window.novinka = novinka
