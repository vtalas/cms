###
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
###


module = angular.module("clientModule",['ngResource','templateExt'])

module.run ()->
	1
#console.log("run clientModule")

module.config ($provide, $routeProvider)->
  $provide.factory "appSettings", () ->
    setings =
      applicationId: "7683508e-0941-4561-b9a3-c7df85791d23",
      serverUrl: "http://localhost\\:62728"

  $provide.factory "clientApi", ($resource,appSettings) ->
    defaults =
      applicationId: appSettings.applicationId,

    actions =
		  getJson: { method: 'GET' ,isArray:false, params: {action : "grids"}},

    proj = $resource(appSettings.serverUrl+"/client/json/:applicationId/:link?callback=JSON_CALLBACK",defaults, actions)
    proj

  $routeProvider
    .when('/link/:link', { controller: clientPageCtrl, templateUrl: 'clientPage-template' })
    .otherwise( redirectTo: '/list' )

  1


module.directive "gridelement", (gridtemplate,gridtemplateClient,$compile,$templateCache)->

  directiveDefinitionObject =
    scope: {  grid: "=", gridelement: "=" }
    link: (scope, iElement, tAttrs, controller) ->
      type = scope.gridelement.Type
      sablona = $("#"+type+"-template").html()
      compiled = $compile(sablona)(scope)
      iElement.html(compiled)

  directiveDefinitionObject



clientPageCtrl = ($scope,$routeParams,clientApi) ->

  p = $routeParams

  clientApi.getJson({link:p.link }, (data)->
    $scope.data = data
  )

window.clientPageCtrl = clientPageCtrl






