var gridPage = function ($scope, gridEelementApi, appSettings) {

	$scope.addWithType = function (item, gridid, elements, newtype, event) {
		event.preventDefault();
		item.Type = newtype;
		$scope.addToEnd(item, gridid, elements);
	};

	$scope.addToEnd = function (item, gridId, elements) {
		item.Position = elements.length;
		$scope.add(item, gridId, elements);
	};

	$scope.add = function (item, gridId, elements) {
		gridEelementApi.post({
			gridId: gridId
		}, item, function (data) {
			elements.push(data);
		});
	};

	$scope.remove = function (item) {
		gridEelementApi.delete({ id: item.Id },
			function () {
				item.Id = 0;
				item.Content = "";
				//refresh - preopocitani poradi radku
				$scope.$emit("refreshgrid");
			});
	};

	$scope.$on("gridelement-save", function (e, gridelemnt) {
		$scope.save(gridelemnt);
	});

	$scope.save = function (item) {
		var copy = jQuery.extend(true, {}, item);

		if (angular.isObject(copy.Content)) {
			copy.Content = JSON.stringify(copy.Content);
		}

		gridEelementApi.put(copy);
	};
};
