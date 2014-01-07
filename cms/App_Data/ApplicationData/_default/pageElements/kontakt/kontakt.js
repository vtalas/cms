
var kontaktCtrl = function ($scope) {
	kontaktCtrl.$inject = ["$scope"];

	gridelement = $scope.$parent.gridelement;

	$scope.$on("ngcClickEdit.showPreview", function(e, data) {
		///console.log(data, gridelement);
//		$scope.$emit("gridelement-save", gridelement);
	});

}
