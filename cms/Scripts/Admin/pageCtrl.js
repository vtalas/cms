function _newitem(position) {
	var newitem = { Id: 0, Width: 12, Type: "text", Position: position, Edit: 0 };
	return newitem;
}

var pageCtrl = ['$scope', '$http', '$routeParams', 'appSettings', 'GridApi', "$json", function ($scope, $http, $routeParams, appSettings, GridApi, $json) {
	var gridId = $routeParams.Id,
		x;

	$scope.grid = {};

	var getGrid = function () {
		$scope.grid = x.getGrid(gridId);
		console.log($scope.grid);
	};

	GridApi.load().success(function (data) {
		x = new Picus(data, $json);
		getGrid();
	});

	$scope.newItem = {
		Id: 0,
		Type: "text",
		Edit: 0,
		Width:12
	};

	$scope.$on("refreshgrid", function () {
		console.log("refreshgrid ");
		getGrid();
	});

	$scope.$on("setCultureEvent", function () {
		console.log("pageCtrl set culture");
		getGrid();
	});


	$scope.availableTemplates = [
		{ value: "text", name: "Text" },
		{ value: "album", name: "Album" },
		{ value: "simplehtml", name: "Markdown" },
		//{ value: "gdataalbum", name: "Google Album" },
		{ value: "clientspecific", name: "Klientská šablona" }
	];


	$scope.addWithType = function (item, gridid, newtype, event) {
		event.preventDefault();
		item.Type = newtype;
		$scope.addToEnd(item, gridid);
	};

	$scope.addToEnd = function (item, gridId) {
		$scope.add(item, gridId);
	};

	$scope.add = function (item, gridId) {
		item.Id = "el_" + ($scope.grid.GridElements.length + 1);
		$scope.grid.GridElements.push(item);
		x.save();
	};

	$scope.remove = function (item) {
//		gridEelementApi.remove({ id: item.Id },
//			function () {
//				item.Id = 0;
//				item.Content = "";
//				//refresh - preopocitani poradi radku
//				$scope.$emit("refreshgrid");
//			});
	};

	var broadcastTime = function() {
		$scope.$broadcast("gridpage-tick", new Date() );
		timeout();
	};
	var timeout = function () {	setTimeout(broadcastTime, 60000 );};
	timeout();


	$scope.$on("gridelement-save", function (e, gridelement) {
		x.save();
	});

	$scope.$on("gridname.showPreview", function (e, gridelemnt) {
		x.save();
	});

	$scope.save = function () {
		x.save();
	};

	$scope.showHelp = function(gridelement) {
		var help = gridelement.help || false;
		gridelement.help = !help;
	};



}];