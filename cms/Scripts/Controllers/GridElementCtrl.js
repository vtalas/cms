var gridElementCtrl = function ($scope, gridEelementApi, appSettings) {

	$scope.addWithType = function (item, gridid, elements, newtype, event) {
		event.preventDefault();
		item.Type = newtype;
		$scope.add(item, gridid, elements);
	};

	$scope.add = function (item, gridId, elements) {
		gridEelementApi.post({
			gridId: gridId
		}, item, function (data) {
			elements.push(data);
			//TODO: nevyvola se broadcast
			$scope.edit(data);
		});
	};

	$scope.remove = function (item) {
		gridEelementApi.delete({ id: item.Id },
			function () {
				item.Id = 0;
				item.Edit = 0;
				item.Content = "";
				//refresh - preopocitani poradi radku
				$scope.$emit("refreshgrid");
			});
	};

	$scope.edit = function (item) {
		$scope.$broadcast("gridelement-edit");
		if (item.Id !== 0) {
			item.Edit = 1;
		}
	};

	$scope.save = function (item) {
		var copy = jQuery.extend(true, {}, item);

		if (angular.isObject(copy.Content)) {
			copy.Content = JSON.stringify(copy.Content);
		}

		gridEelementApi.put( copy ,
			function () {
				item.Edit = 0;
			});
	};
};
