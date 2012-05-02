///<reference path="../angular-1.0.0rc6.js"/>


function show($scope, $http) {
	var self = this;

	$http({ method: 'POST', url: '/adminApi/index' })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data);
		})
		.error(function (data, status, headers, config) {
		});


		$scope.thumb = function () {
			console.log($scope);
		};

}
