###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

textlocalized = ($scope, $http, appSettings) ->
	textlocalized.$inject = [ "$scope", "$http", "appSettings"]
#	console.log(appSettings)
	1

window.textlocalized = textlocalized
