//function _newitem(position) {
//	var newitem = { Id: 0, Width: 12, Type: "text", Position: position, Edit: 0 };
//	return newitem;
//}
var generateUid = function (separator) {
	var delim = separator || "-";

	function S4() {
		return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
	}

	return (S4() + S4() + delim + S4() + delim + S4() + delim + S4() + delim + S4() + S4() + S4());
};
var pageCtrl = ['$scope', '$http', '$routeParams', 'appSettings', 'GridApi', "$json", function ($scope, $http, $routeParams, appSettings, GridApi, $json) {
	var gridId = $routeParams.Id,
		x;

	$scope.grid = {};

	var getGrid = function () {
		$scope.grid = x.getGrid(gridId);
	};

	GridApi.load().success(function (data) {
		x = new Picus(data, $json);
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


	$scope.addWithType = function (item, gridid, templateItem, event) {
		event.preventDefault();
		item.Type = templateItem.value;
		item.Name= templateItem.name;
		$scope.addToEnd(item, gridid);
	};

	$scope.addToEnd = function (item, gridId) {
		$scope.add(item, gridId);
	};

	$scope.add = function (item) {
		item.Id = generateUid("");
		$scope.grid.GridElements.push(item);
		x.save();
	};

	$scope.remove = function (item) {
		var index = $scope.grid.GridElements.indexOf(item);
		$scope.grid.GridElements.splice(index, 1);
		item.Id = 0;
		item.Content = "";
		x.save();
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
		x.save();
	});

	$scope.$on("gridname.showPreview", function () {
		x.save();
	});

	$scope.save = function () {
		x.save();
	};

	$scope.showHelp = function (gridelement) {
		var help = gridelement.help || false;
		gridelement.help = !help;
	};


}];