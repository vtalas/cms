###
@reference ../jquery-1.7.2.js
@reference ../angular.js
###

bootstrap = ($scope, $http) ->
  #novinka.$inject = [ "$scope", "$http", "appSettings" ]
  console.log "kjabsdjkasd"
  $scope.xxx = "bootstrap"

  $http(
    method: "POST"
    url: "/bootstrap/Current"
  ).success((data, status, headers, config) ->
      $scope.data = data;
      console.log data
  )

  $scope.refresh = ->
    $http(
      method: "POST"
      url: "/bootstrap/Refresh"
      data: $scope.data
    ).success((data, status, headers, config) ->
      console.log data
    )

  1
window.bootstrap = bootstrap
