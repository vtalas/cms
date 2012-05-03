///<reference path="../angular-1.0.0rc6.js"/>



function index($scope, $http) {
	var self = this;

	$http({ method: 'POST', url: '/adminApi/aaa/Applications' })
		.success(function (data, status, headers, config) {
			$scope.data = data;
		})
		.error(function (data, status, headers, config) {
		});



		$scope.add = function () {
			$scope.data.push({ Name: $scope.newitem, Id: 0 });
			$scope.newitem = '';
		};

}
