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
#      applicationId: "86199013-5887-4743-89dd-29ddc5bc7df7",
      serverUrl: "http://localhost\\:62728"

  $provide.factory "clientApi", ($resource,appSettings) ->
    defaults =
      applicationId: appSettings.applicationId,
    actions =
	    gridpageJson: { method: 'GET' ,isArray:false}
    proj = $resource(appSettings.serverUrl+"/client/json/:applicationId/:link?callback=JSON_CALLBACK",defaults, actions)
    proj

  $routeProvider
    .when('/link/:link', { controller: linkCtrl, templateUrl: 'link-template' })
    .when('/gallery/:link', { controller: galleryCtrl, templateUrl: 'link-template' })

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



appController = ($scope, $routeParams,clientApi)->
  $scope.thumbs  = []
  $scope.refresh = (lines)->
    $scope.thumbs = lines

window.appController = appController

###########################

linkCtrl = ($scope,$routeParams,clientApi) ->
  p = $routeParams
  clientApi.gridpageJson({link:p.link }, (data)->
    $scope.data = data
  )
window.linkCtrl = linkCtrl

###########################

galleryCtrl = ($scope,$routeParams,clientApi) ->
  p = $routeParams


  clientApi.gridpageJson({link:p.link }, (data)->
    $scope.$parent.refresh(data.Lines)
    $scope.data = data
  )
window.linkCtrl = linkCtrl












































