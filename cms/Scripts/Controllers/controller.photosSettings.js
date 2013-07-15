var photoSettings = ["$scope", "$location", "settingsApi", function ($scope, $location, settingsApi) {

	$scope.data = [];
	
	var getDefaults = function () {
		return [
			{
				Type: 0,
				Value: 100,
				IsSquare: false
			},
			{
				Type: 1,
				Value: 300,
				IsSquare: false
			},
			{
				Type: 2,
				Value: 400,
				IsSquare: true
			}
		];
	};

	settingsApi.getPhotosSettings(function (data) {
		var defaults = getDefaults();
		$scope.data[0] = data[0] ? data[0] : defaults[0];
		$scope.data[1] = data[1] ? data[1] : defaults[1];
		$scope.data[2] = data[2] ? data[2] : defaults[2];
	});

	$scope.save = function () {
		settingsApi.putPhotosSettings($scope.data, function () {
			console.log("aksjbdkj")
		});
	};
}];