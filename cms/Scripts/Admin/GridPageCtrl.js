

function GridPageCtrl($scope, $http, $routeParams, appSettings) {
	$scope.data = {};
	var self = this;

	$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/GetGrid/' + $routeParams.Id })
		.success(function (data, status, headers, config) {
			$scope.data = data;
			console.log(data, "edit");
		})

		.error(function (data, status, headers, config) {

		});


	$scope.add = function (item) {
		var newitem = item ? addItem(item) : addNewline();

		$http({
			method: 'POST',
			url: '/adminApi/' + appSettings.Name + '/AddGridElement',
			data: {
				data: newitem,
				gridId: $scope.data.Id
			}
		}).success(function (data, status, headers, config) {
			console.log(data, "ser");
			if (data.Line >= $scope.data.Lines.length) {
				$scope.data.Lines.push([]);
			}
			$scope.data.Lines[data.Line][data.Position] = data;
		});
		$scope.newName = '';
	};
	$scope.remove = function (item) {
		console.log(item);
		var index = $scope.data.Lines[item.Line].indexOf(item);

		if (index != -1) $scope.data.Lines[item.Line].splice(index, 1);

		$http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/DeleteGridElement', data: item })
				.success(function (data, status, headers, config) {
					console.log("deleted");
				});
	};

	$scope.edit = function (item, $element) {
		item.Edit = 1;
	};

	$scope.save = function (item) {
		var data = jQuery.extend(true, {}, item);

		if (angular.isObject(data.Content))
			data.Content = JSON.stringify(data.Content);

		$http({
			method: 'POST',
			url: '/adminApi/' + appSettings.Name + '/UpdateGridElement/' + $scope.data.Id,
			data: data
		}).success(function () {
			item.Edit = 0;

		});

	};


	var addNewline = function () {
		var newlineIndex = $scope.data.Lines.length;
		var newline = [];
		var newitem = { Width: 12, Type: $scope.newName, Line: newlineIndex };

		newline.push(newitem);

		return newitem;
	};
	var addItem = function (newitem) {
		return newitem;
	};
}
