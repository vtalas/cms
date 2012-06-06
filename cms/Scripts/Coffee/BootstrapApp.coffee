###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

module = angular.module("bootstrapApp", [  ])
module.config [ "$routeProvider", "$provide", ($routeProvider, $provide) ->

  $routeProvider.when("/csstest",
    controller: aaaController
    template: "/Content/csstest.html"
  ).when("/aaa",
    controller: aaaController
    template: "/Content/aaa.html"
  ).when("/bootswatch",
    controller: aaaController
    template: "/Content/bootswatch.html"
  )


]
module.directive "bootstrapelem", [ () ->

  directiveDefinitionObject =
    link: (scope, el, tAttrs, controller) ->
      scope.$watch(tAttrs.ngModel, ()->
        val = el.val()
        if val[0] == "#" then el.css "background", val
        console.log val[0]
      )
      1

  directiveDefinitionObject
]


#window.bootstrap = bootstrap

aaaController = ($scope, $http) ->


