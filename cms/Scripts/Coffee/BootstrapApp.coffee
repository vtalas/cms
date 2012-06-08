###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###


module = angular.module("bootstrapApp", [  ])

module.config  ($routeProvider, $provide,$filterProvider) ->
  $filterProvider.register('typevalue', ()->
    (data, type)->
      x = {}
      #vyfiltruj to co je 'type' a zaroven nezacina na '@'
      for key,prop of data
        x[key] = prop.value if prop.type is type && prop.value[0] isnt "@"
      x
  )
  $filterProvider.register('refs', ()->
    (data, type)->
      x = {}
      #vyfiltruj to co je 'type' a zaroven ZACINA na '@'
      for key,prop of data
        x[key] = prop.value if prop.type is type && prop.value[0] is "@"
      x
  )

  $provide.factory("datajson", ()->
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

module.directive "bootstrapelem", (datajson,colorsrefonly,colorsonly,$filter) ->

  directiveDefinitionObject =
    scope: {bootstrapelem: "accessor" }
    link: (scope, el, tAttrs, controller) ->

      all = scope.$parent.data
      scope.$watch('bootstrapelem().value', ()->
        a = scope.bootstrapelem()

        if a.type == "color"
          el.width "50px"
          if a.value[0] != "@"
            el.css "background", a.value

          ccc =  $filter("typevalue")(all, "color")
          if a.value[0] == "@"
            r = ccc[a.value.substr(1)]
            el.css "background", r if r



      )

  directiveDefinitionObject



#window.bootstrap = bootstrap

aaaController = ($scope, $http) ->
  # Type here!

#  days =
#    monday: 1
#    tuesday: 2
#    wednesday: 3
#    thursday: 4
#    friday: 5
#    saturday: 6
#    sunday: 7
#
#  for xx of days
#    console.log xx


#$scope.colorsrefonly = colorsrefonly
1



