###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

text = ($scope, $http) ->
  text.$inject = [ "$scope", "$http" ]

  $scope.xxx = "xxxx"
  1

window.text = text
