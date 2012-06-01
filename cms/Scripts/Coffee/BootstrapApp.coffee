###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###


module = angular.module("bootstrapApp", [  ])

module.config  ($routeProvider, $provide,$filterProvider) ->
  $filterProvider.register('typevalue', ()->
    (data, type)->
      console.log "kjbsadksjbd"
      x = []
      #vyfiltruj to co je 'type' a zaroven nezacina na '@'
      x[num.name] = num.value for num in data.grays  when num.type is type && num.value[0] isnt "@"
      x
  )
  $filterProvider.register('refs', ()->
    (data, type)->
      console.log "krefsd"
      x = []
      #vyfiltruj to co je 'type' a zaroven zacina na '@'
      x[num.name] = num.value for num in data.scaffolding  when num.type is type && num.value[0] is "@"
      x
  )

  $provide.factory("datajson", ($filter)->
    d = $.parseJSON angular.element("html").data("modeldata")
    d
  )
  $provide.factory("colorsonly", ($filter,datajson)->
    a =  $filter("typevalue")(datajson, "color")
    a
  )

  $provide.factory("colorsrefonly", ($filter,datajson)->
    a =  $filter("refs")(datajson, "color")
    a
  )

  $routeProvider.when("/csstest",
    controller: aaaController
    template: "/Content/csstest.html"
  ).when("/aaa",
    controller: aaaController
    template: "/Content/aaa.html"
  ).when("/bootswatch",
    controller: aaaController
    template: "/Content/bootswatch.html"
  ).otherwise redirectTo: '/aaa'

module.directive "bootstrapelem", (datajson,colorsonly) ->

  directiveDefinitionObject =
    link: (scope, el, tAttrs, controller) ->
      console.log colorsonly,datajson

      all = scope.$parent.data
      scope.$watch(tAttrs.ngModel, ()->
        a = scope.item

        if a.type == "color" && a.value[0] == "#"
          el.css "background", a.value

        if a.type == "color" && a.value[0] == "@"
          #console.log "ref",colorsonly[a.value.substr(1)]
          r = colorsonly[a.value.substr(1)]
          el.css "background", r if r

      )
      1

  directiveDefinitionObject



#window.bootstrap = bootstrap

aaaController = ($scope, $http,colorsrefonly) ->
  $scope.colorsrefonly = colorsrefonly
1



