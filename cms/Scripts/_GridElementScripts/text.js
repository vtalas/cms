///<reference path="../angular-1.0.0rc8.js"/>
///<reference path="../jquery-1.7.2.js"/>

function text($scope, $http, appSettings) {
	$scope.data = $scope.$parent.item; 
	//console.log("text controller", $scope, $scope.item);

	$scope.save = function () {
		var data = jQuery.extend(true, {}, $scope.data);
		if (angular.isObject(data.Content))
			data.Content = JSON.stringify(data.Content);

		$http({
			method: 'POST',
			url: '/adminApi/' + appSettings.Name + '/UpdateGridElement/' + $scope.data.Id,
			data: data
		});

	};
}
