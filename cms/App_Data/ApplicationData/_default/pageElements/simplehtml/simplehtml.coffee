###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

simplehtml = ($scope, $http) ->
    simplehtml.$inject = [ "$scope", "$http" ]

    gridelement = $scope.$parent.gridelement;

    $scope.$on "ngcClickEdit-showPreview", (e, data) ->
        $scope.$emit("gridelement-save", gridelement);


    converter = new Showdown.converter()

    toHtml = (markdown) ->
        converter.makeHtml markdown if markdown

    $scope.ContentToHtml = ->
        toHtml gridelement.Resources.text.Value
    1

window.simplehtml = simplehtml