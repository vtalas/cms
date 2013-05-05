###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

simplehtml = ($scope, $http) ->
	simplehtml.$inject = [ "$scope", "$http" ]

	gridelement = $scope.$parent.gridelement;

	$scope.$on "ngcClickEdit.showPreview", (e, data) ->
		$scope.$emit("gridelement-save", gridelement);


	converter = new Showdown.converter()

	toHtml = (markdown) ->
		converter.makeHtml markdown if markdown

	$scope.ContentToHtml = ->
		text = (if gridelement.Resources.text then gridelement.Resources.text.Value else "")
		toHtml text
1

window.simplehtml = simplehtml