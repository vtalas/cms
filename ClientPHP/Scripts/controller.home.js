function homeController($scope, cmsApi) {
	var api = new ApiWrapper(cmsApi);



	api.getPages().then(function (data) {
   		$scope.pages = data;
		console.log(data)
	});



}