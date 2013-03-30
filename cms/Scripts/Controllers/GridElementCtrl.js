var gridElementCtrl = function ($scope, gridEelementApi, appSettings) {

	$scope.addWithType = function (item, gridid, elements, newtype, event) {
		event.preventDefault();
		item.Type = newtype;
		$scope.add(item, gridid, elements);
	};
	var x = gridEelementApi.get({id: "c78ee05e-1115-480b-9ab7-a3ab3c0f6643"}, function(data) {
		console.log("kjsdfnlkjsa", data);
	});


	$scope.add = function (item, gridId, elements) {
		gridEelementApi.post({
			applicationId: appSettings.Id,
			data: item,
			gridId: gridId
		}, function (data) {
			elements.push(data);
			//TODO: nevyvola se broadcast
			$scope.edit(data);
		});
	};

	$scope.remove = function (item, gridId) {
		gridEelementApi.delete({ applicationId: appSettings.Id, data: item, gridId: gridId },
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

		gridEelementApi.put({ applicationId: appSettings.Id, data: copy },
			function () {
				item.Edit = 0;
			});
	};
};
