///<reference path="../angular-1.0.0rc7.js"/>


function show($scope, $http) {
	var self = this;
	var e = angular.element(".show");
	var appName = e.data("application-name");

	$http({ method: 'POST', url: '/adminApi/'+appName+'/show' })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data);
		})

		.error(function (data, status, headers, config) {

		});

		$scope.edit = function (parameters) {
			console.log(parameters);
		};

}
