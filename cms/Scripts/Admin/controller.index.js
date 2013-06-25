var index = ['$scope', '$http', '$element', function ($scope, $http, $element) {
	$scope.data = $element.data("model");
	console.log($scope.data)
	$scope.add = function () {
		var newitem = { Name: $scope.newitem };


		$http({ method: 'POST', url: '/api/00000000-0000-0000-0000-000000000000/adminapi/AddApplication', data: newitem })
			.success(function (data, status, headers, config) {
			    $scope.data.push(data);
			})
			.error(function (data, status, headers, config) {

			});
		$scope.newitem = '';
	};

} ];
