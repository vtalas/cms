///<reference path="../angular.js"/>
///<reference path="../jquery-1.7.2.js"/>

function text($scope, $http, appSettings) {
	$scope.item = $scope.$parent.data
	//console.log("text controller", $scope);
}
text.$inject = ['$scope', '$http', 'appSettings'];


