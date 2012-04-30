///<reference path="../showdown.js"/>
///<reference path="../angular-1.0.0rc6.js"/>


function sd($scope, $http) {
	var converter = new Showdown.converter();
	var self = this;
	var toHtml = function(markdown) {
		return converter.makeHtml(markdown);
	};

	$http({ method: 'POST', url: '/clientapi/test1/ContentElement' })
		.success(function (data, status, headers, config) {
			$scope.data = data;
		})
		.error(function (data, status, headers, config) {
		});
	
	$scope.headerHtml = function() {
		return toHtml($scope.data.header);
	};
	
	$scope.textHtml = function() {
		return toHtml($scope.data.text);
	};
	
	$scope.thumb = function() {
		return $scope.data.thumb;
	};

}
