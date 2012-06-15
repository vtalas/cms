###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###


module = angular.module("bootstrapApp", [  ])

module.config  ($routeProvider, $provide,$filterProvider) ->
  $filterProvider.register('nameType', ()->
    (data, type, name)->
      x = {}
      #vyfiltruj to co je 'type' a zaroven nezacina na '@'
      for key,prop of data
        x[key] = prop.value if prop.type is type && key.indexOf(name.substr(1)) isnt -1
      x
  )
  $filterProvider.register('typevalue', ()->
    (data, type, type2)->
      x = {}
      #vyfiltruj to co je 'type' a zaroven nezacina na '@'
      for key,prop of data
        x[key] = prop.value if prop.type is type || prop.type is type2 && prop.value[0] isnt "@"
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

  $routeProvider.when("/csstest",
    controller: aaaController
    template: "/Content/csstest.html"
  ).when("/bootswatch",
    controller: aaaController
    template: "/Content/bootswatch.html"
  ).otherwise redirectTo: '/csstest'

module.directive "bootstrapelem", (datajson,$filter) ->

  directiveDefinitionObject =
    scope: {bootstrapelem: "accessor" }
    controller : ($scope, $element )->
      $scope.test = ($event)->
        all = $scope.$parent.data
        a = $scope.bootstrapelem()

        basiccolors=  $filter("nameType")(all, "basiccolor", a.value )
        if a.value[0] is "@" && a.value.length is 1
          console.log(basiccolors, "is aaa")

        console.log(basiccolors,a.value.length, a.value[0])
      1
    link: (scope, el, tAttrs, controller) ->
      all = scope.$parent.data
      scope.$watch('bootstrapelem().value', ()->
        a = scope.bootstrapelem()

        switch a.type
          when "color","basiccolor"
            el.width "80px"
            if a.value[0] != "@"
              el.css "background", a.value

            if a.value[0] == "@"
              colorsonly =  $filter("typevalue")(all, "color","basiccolor" )
              r = colorsonly[a.value.substr(1)]
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



