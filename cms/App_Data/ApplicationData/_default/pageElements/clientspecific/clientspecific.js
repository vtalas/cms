
var clientspecific  = function($scope) {
	clientspecific.$inject = ["$scope"];

	gridelement = $scope.$parent.gridelement;

	$scope.$on("ngcClickEdit.showPreview", function(e, data) {
		///console.log(data, gridelement);
//		$scope.$emit("gridelement-save", gridelement);
	});

}
