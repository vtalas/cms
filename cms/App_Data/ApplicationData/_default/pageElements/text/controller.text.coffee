###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

text = ($scope) ->
	text.$inject = [ "$scope" ]

	gridelement = $scope.$parent.gridelement;

	$scope.$on "ngcClickEdit.showPreview", (e, data) ->
        $scope.$emit("gridelement-save", gridelement);


window.text = text