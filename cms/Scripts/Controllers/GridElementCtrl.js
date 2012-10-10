function _newitem(line, type) {
	var newitem = { Id: 0, Width: 12, Type: type, Line: line, Edit: 0 };
	return newitem;
}

var gridElementCtrl = function ($scope, GridApi, appSettings) {
	$scope.addWithType = function (item, newtype, event) {
		event.preventDefault();
		item.Type = newtype;
		$scope.add(item);
	};
	$scope.add = function (item, gridId, lines) {
		GridApi.AddGridElement({
			applicationId: appSettings.Id,
			data: item,
			gridId: gridId
		}, function (data) {
			if (data.Line >= lines.length - 1) {
				var newitem = _newitem(lines.length, item.Type);
				lines.push([newitem]);
			}
			lines[data.Line][data.Position] = data;
			//TODO: nevyvola se broadcast
			$scope.edit(data);
		});
	};

	$scope.remove = function (item, gridId) {
		var line = $scope.$parent.$parent.line;
		GridApi.DeleteGridElement({ applicationId: appSettings.Id, data: item, gridId: gridId },
			function () {
				item.Id = 0;
				item.Edit = 0;
				item.Content = "";
				//refresh - preopocitani poradi radku
				if (line.length === 1) {
					$scope.$emit("refreshgrid");
				}
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
		GridApi.UpdateGridElement({ applicationId: appSettings.Id, data: copy },
			function () {
				item.Edit = 0;
			});
	};
};
