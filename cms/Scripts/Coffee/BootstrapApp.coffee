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
#window.bootstrap = bootstrap

aaaController = ($scope, $http) ->

  1
#window.bootstrap = bootstrap

