///<reference path="../angular.js"/>

var index = ['$scope', '$http', '$element', function ($scope, $http, $element) {
	var self = this;

	$scope.data = JSON.parse(  $element.data("model"));

	$scope.add = function () {
		var newitem = { Name: $scope.newitem };
		$scope.data.push(newitem);

		$http({ method: 'POST', url: '/adminApi/00000000-0000-0000-0000-000000000000/AddApplication', data: newitem })
			.success(function (data, status, headers, config) {

			})
			.error(function (data, status, headers, config) {

			});
		$scope.newitem = '';
	};

} ];
