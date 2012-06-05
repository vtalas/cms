###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

module = angular.module("bootstrapApp", [  ])
module.config [ "$routeProvider", "$provide", ($routeProvider, $provide) ->

  $routeProvider.when("/aaa",
    controller: aaaController
    template: "/Content/aaa.html"
  ).when("/new",
    controller: aaaController
    template: "template/new"
  )

]
#window.bootstrap = bootstrap

aaaController = ($scope, $http) ->

  1
#window.bootstrap = bootstrap

