var mainCtrl = function ($scope,clientApi) {

	$scope.data = {};

	clientApi.getJson({link: "linkTestPage"}, function(data){
		$scope.data = data;
		//console.log($scope.data)
	})


}