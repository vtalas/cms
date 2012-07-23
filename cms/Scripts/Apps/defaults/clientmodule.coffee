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
	    gridpageJson: { method: 'GET' ,isArray:false},
    proj = $resource(appSettings.serverUrl+"/client/json/:applicationId/:link?callback=JSON_CALLBACK",defaults, actions)
    proj

  $provide.factory "GridApi", ($resource,appSettings) ->
    defaults =
      applicationId: appSettings.applicationId,
    actions =
      getGrid: { method: 'GET' ,isArray:false, params : {action: "GetGrid"}}
    proj = $resource(appSettings.serverUrl+"/clientapi/:applicationId/:action/:Id?callback=JSON_CALLBACK",defaults, actions)
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




linkCtrl = ($scope,$routeParams,clientApi) ->
  p = $routeParams
  clientApi.gridpageJson({link:p.link }, (data)->
    $scope.data = data
  )
window.linkCtrl = linkCtrl

###########################

galleryCtrl = ($scope,$routeParams,clientApi, GridApi) ->
  p = $routeParams

  clientApi.gridpageJson({link:p.link }, (data)->
    $scope.$parent.refreshThumbs(data)
#    $scope.data = data

    console.log(data, "xx")
#    GridApi.getGrid({Id:$scope.gridelement.Content.Id}, (data)->
#      $scope.destination = data
#      #    $scope.data = data
#      )

    )



window.linkCtrl = linkCtrl

appController = ($scope, $routeParams)->
  $scope.referenceItems  = {}
  $scope.refreshThumbs = (lines)->
    console.log( lines, "xxx")
    $scope.referenceItems = lines

window.appController = appController

###########################











































