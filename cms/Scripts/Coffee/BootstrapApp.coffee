###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

module = angular.module("bootstrapApp", [  ])

module.config ($routeProvider,$provide,$filterProvider) ->
  $filterProvider.register('nameType', ()->
    (data, type, name)->
      x = []
      #vyfiltruj to co je 'type' a zaroven nezacina na '@'
      for key,prop of data
        x.push("@"+key) if prop.type is type
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
    templateUrl : "/Content/csstest.html"
  ).when("/bootswatch",
    controller: aaaController
    templateUrl : "/Content/bootswatch.html"
  ).otherwise redirectTo: '/csstest'

module.directive "bootstrapelem", (datajson,$filter) ->

  directiveDefinitionObject =
    require : '?ngModel',
    link: (scope, el, tAttrs, controller) ->

      controller.$setViewValue = (val)->
        basiccolors=$filter("nameType")(scope.data, "basiccolor" )
        $(el).typeahead({
                          source:basiccolors,
                          updater: (val)->
                            colorsonly =  $filter("typevalue")(scope.data, "color","basiccolor" )
                            r = colorsonly[val.substr(1)]
                            el.css "background", r if r
                            controller.$viewValue.value = val
                          items:11
                          });
        controller.$viewValue.value = val

      controller.$render = ()->
        el.val(controller.$viewValue.value || '');

        a = controller.$viewValue
        switch a.type
          when "color","basiccolor"
            el.width "80px"
            if a.value[0] != "@"
              el.css "background", a.value

            if a.value[0] == "@"
              colorsonly =  $filter("typevalue")(scope.data, "color","basiccolor" )
              r = colorsonly[a.value.substr(1)]
              el.css "background", r if r
  directiveDefinitionObject

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



