//function EditCtrl($scope, $route, $routeParams, $location, $http, $log, $templateCache, appSettings) {
var EditCtrl = ['$scope', '$route', '$routeParams', 'location', '$http', '$log', '$templateCache', 'appSettings', function ($scope, $route, $routeParams, $location, $http, $log, $templateCache, appSettings) {
	$scope.data = {};

	$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/GetGridElement/' + $routeParams.GridElementId })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data, "edit");
		})

		.error(function (data, status, headers, config) {

		});
} ]

