###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

text = ($scope, $http, appSettings) ->
  text.$inject = [ "$scope", "$http", "appSettings"]
#  console.log(appSettings)

  $scope.xxx = "xxxx"
  1

window.text = text
