var photoSettings = ["$scope", "$location", "settingsApi", function ($scope, $location, settingsApi){

	var getDefaults = function () {
		return [
			{
				ImageSizeType: 0,
				Value: 100,
				IsSquare: false
			},
			{
				ImageSizeType: 0,
				Value: 300,
				IsSquare: false
			},
			{
				ImageSizeType: 0,
				Value: 400,
				IsSquare: true
			}
		];
	};

	settingsApi.getPhotosSettings(function (data) {
		$scope.data = data.length === 0 ? getDefaults() : data;
	});


	$scope.save = function () {
		settingsApi.putPhotosSettings($scope.data, function() {
			console.log("aksjbdkj")
		});
	};
}];