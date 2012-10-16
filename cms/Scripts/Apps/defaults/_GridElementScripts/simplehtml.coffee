###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

simplehtml = ($scope, $http) ->
	text.$inject = [ "$scope", "$http" ]

	converter = new Showdown.converter()

	toHtml = (markdown) ->
		converter.makeHtml markdown if markdown

	$scope.ContentToHtml = ->
		toHtml $scope.gridelement.Content
	1

window.simplehtml = simplehtml
