var photoSettings = ["$scope", "$location", "settingsApi", function ($scope, $location, settingsApi){

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
		$scope.data = data.length === 0 ? getDefaults() : data;
	});


	$scope.save = function () {
$scope.data[0].Type = 2;
		settingsApi.putPhotosSettings($scope.data, function() {
			console.log("aksjbdkj")
		});
	};
}];