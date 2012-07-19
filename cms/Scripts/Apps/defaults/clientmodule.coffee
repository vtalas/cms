###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###


module = angular.module("clientModule",['ngResource','templateExt'])

module.run ()->
	console.log("run clientModule")

module.config ($provide)->
	$provide.factory "appSettings", () ->
		applicationId: "7683508e-0941-4561-b9a3-c7df85791d23"

	$provide.factory "clientApi", ($resource,appSettings) ->
		defaults =
			applicationId: appSettings.applicationId

		actions =
			getJson: { method: 'GET' ,isArray:false, params: {action : "grids"}},

		proj = $resource("http://localhost\\:62728/client/json/:applicationId/:link?callback=JSON_CALLBACK",defaults, actions)
		proj
	1

module.directive "gridelement", (gridtemplate,$compile)->
	directiveDefinitionObject =
		scope: {  grid: "=", gridelement: "=" }
#		controller:GridElementCtrl
		templateUrl:"novinka-template"
		link: (scope, iElement, tAttrs, controller) ->
#			sablona = gridtemplate(scope.gridelement.Type)
#
#			compiled = $compile(sablona)(scope)
#			iElement.html( "type---" + scope.gridelement.Type)
			console.log("askjbdasjkbd")
			1


	directiveDefinitionObject







