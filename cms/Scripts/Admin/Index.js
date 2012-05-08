///<reference path="../angular-1.0.0rc8.js"/>


function index($scope, $http) {
	var self = this;

	$http({ method: 'POST', url: '/adminApi/null/Applications' })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data);
		})
		.error(function (data, status, headers, config) {
		});

		$scope.add = function () {
			var newitem = { Name: $scope.newitem };
			$scope.data.push(newitem);

			$http({ method: 'POST', url: '/adminApi/null/AddApplication', data: newitem })
				.success(function (data, status, headers, config) {
				
				})
				.error(function (data, status, headers, config) {
				
				});
			$scope.newitem = '';
		};

}
