//function _newitem(position) {
//	var newitem = { Id: 0, Width: 12, Type: "text", Position: position, Edit: 0 };
//	return newitem;
//}
var pageCtrl = ['$scope', '$http', '$routeParams', 'appSettings', 'GridApi', "$json","$window", function ($scope, $http, $routeParams, appSettings, GridApi, $json, $window) {
	var gridId = $routeParams.Id,
		gridObject;

	$scope.grid = {};

	var getGrid = function () {
		$scope.grid = gridObject.getGrid(gridId);
	};

	GridApi.load().success(function (data) {
		gridObject = new Picus(data, $json);
		getGrid();
	});

	$scope.availableTemplates = [
		{ value: "text", name: "Text" },
		{ value: "album", name: "Album" },
		{ value: "simplehtml", name: "Markdown" },
		{ value: "kontakt", name: "Kontakt" },
		//{ value: "gdataalbum", name: "Google Album" },
		{ value: "clientspecific", name: "Klientská šablona" }
	];
	$scope.newItem = {
		Id: 0,
		Type: $scope.availableTemplates[0].value,
		Name: $scope.availableTemplates[0].name,
		Edit: 0,
		Width: 12
	};

	$scope.$on("refreshgrid", function () {
		getGrid();
	});

	$scope.$on("setCultureEvent", function () {
		getGrid();
	});


	$scope.addWithType = function (item, templateItem, event) {
		event.preventDefault();
		item.Type = templateItem.value;
		item.Name= templateItem.name;
		$scope.addToEnd(item);
	};

	$scope.addToEnd = function (item) {
		$scope.add(item);
		$(window).scrollTop($(document).height());
	};

	$scope.add = function (item) {
		var elem = new GridElement({type: item.Type});
		$scope.grid.GridElements.push(elem);
		gridObject.save();
	};

	$scope.remove = function (item) {
		var index = $scope.grid.GridElements.indexOf(item);
		$scope.grid.GridElements.splice(index, 1);
		item.Id = 0;
		item.Content = "";
		gridObject.save();
	};

	var broadcastTime = function () {
		$scope.$broadcast("gridpage-tick", new Date());
		timeout();
	},
	timeout = function () {
		setTimeout(broadcastTime, 60000);
	};
	timeout();

	$scope.$on("gridelement-save", function () {
		gridObject.save();
	});

	$scope.$on("gridname.showPreview", function () {
		gridObject.save();
	});

	$scope.save = function () {
		gridObject.save();
	};
3
	$scope.showHelp = function (gridelement) {
		var help = gridelement.help || false;
		gridelement.help = !help;
	};

	$scope.newGroupName = "";
	$scope.addNewGroup = function () {
		var groups = $scope.grid.groups.items,
			id = 0;

		for (var i = 0; i < groups.length; i++) {
			id = Math.max(id, groups[i].id);
		}
		id++;
		groups.push({id: id, name: $scope.newGroupName});
		gridObject.save();
		$scope.newGroupName = "";
	};

	$scope.deleteGroup = function (id) {
		var groups = $scope.grid.groups.items;

		for (var i = 0; i < groups.length; i++) {
			if (id === groups[i].id){
				groups.splice(i,1);
				gridObject.save();
				return ;
			}
		}
	};
}];