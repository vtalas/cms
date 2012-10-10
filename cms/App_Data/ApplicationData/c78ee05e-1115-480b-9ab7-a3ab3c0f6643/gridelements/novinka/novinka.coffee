###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

novinka = ($scope) ->
  novinka.$inject = [ "$scope" ]

  $scope.gridelement.Content = angular.fromJson($scope.gridelement.Content) or { header: "",  text: ""}

  converter = new Showdown.converter()

  toHtml = (markdown) ->
    converter.makeHtml markdown if markdown

  $scope.headerHtml = ->
    toHtml $scope.gridelement.Content.header

  $scope.textHtml = ->
    toHtml $scope.gridelement.Content.text

  $scope.thumb = ->
    $scope.gridelement.thumb


window.novinka = novinka
