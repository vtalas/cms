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
      applicationId: "c78ee05e-1115-480b-9ab7-a3ab3c0f6643",
      serverUrl: "http://localhost\\:62728"
      currentGallery: "s"
      currentSubGallery: "ss1"

  $provide.factory "clientApi", ($resource,appSettings) ->
    defaults =
      applicationId: appSettings.applicationId,
    actions =
	    gridpageJson: { method: 'GET' ,isArray:false},
    proj = $resource(appSettings.serverUrl+"/client/json/:applicationId/:link?callback=JSON_CALLBACK",defaults, actions)
    proj

  $routeProvider
    .when('/link/:link', { controller: linkCtrl, templateUrl: 'link-template' })
    .when('/gallery/:link/:xxx', { controller: galleryCtrl, templateUrl: 'link-template' })
    .otherwise('/link/profil', { controller: galleryCtrl, templateUrl: 'link-template' })
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
#########################################################################################################
galleryCtrl = ($scope,$routeParams,clientApi, appSettings) ->
  p = $routeParams

  if p.xxx
    clientApi.gridpageJson({link:p.xxx }, (data)->
      $scope.data = data
    )
  # aby se zbytecne nepreenacitalo 's' pri kliku s/ss1 -> s/ss2
  return if appSettings.currentgallery  == p.link
  appSettings.currentgallery = p.link

  clientApi.gridpageJson({link:p.link }, (data)->
    $scope.$parent.refreshGalleryThumbs(data)
  )
window.galleryCtrl = galleryCtrl
#########################################################################################################
appController = ($scope,appSettings,clientApi,$routeParams, $location, $route)->

  $scope.refreshGalleryThumbs = (lines)->
    $scope.galleryThumbs = lines

  $scope.$on("$routeChangeSuccess", ()->
    if !$routeParams.xxx && !$scope.galleryThumbs
      clientApi.gridpageJson({link:appSettings.currentGallery }, (data)->
        $scope.galleryThumbs = data
      )
  )

window.appController = appController
###########################







































































