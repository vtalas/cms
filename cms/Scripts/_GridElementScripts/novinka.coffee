###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

novinka = ($scope, $http, appSettings) ->
  novinka.$inject = [ "$scope", "$http", "appSettings" ]


  $scope.item = $scope.$parent.data

#  console.log($scope.data)

  $scope.data.Content = angular.fromJson($scope.item.Content) or { header: "",  text: ""}

  converter = new Showdown.converter()
  self = this

  toHtml = (markdown) ->
    x = true;
    converter.makeHtml markdown if markdown

  $scope.headerHtml = ->
    toHtml $scope.item.Content.header

  $scope.textHtml = ->
    toHtml $scope.item.Content.text

  $scope.thumb = ->
    $scope.item.thumb

  $scope.add = ->

window.novinka = novinka
