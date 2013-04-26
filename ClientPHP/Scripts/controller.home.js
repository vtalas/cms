function homeController($scope, cmsApi) {
	var api = new ApiWrapper(cmsApi);

	api.getPage("testPage_link");

	console.log("kajsbdjkasbd")


}