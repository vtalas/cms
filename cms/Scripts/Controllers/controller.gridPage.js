var gridPage = ["$scope", "gridEelementApi", "GridApi" ,function ($scope, gridEelementApi, GridApi) {


	$scope.availableTemplates = [
		{ value: "text", name: "Text" },
		{ value: "album", name: "Album" },
		{ value: "simplehtml", name: "Markdown" },
		{ value: "gdataalbum", name: "Google Album" },
		{ value: "clientspecific", name: "Klientská šablona" }
	];

	
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
		gridEelementApi.remove({ id: item.Id },
			function () {
				item.Id = 0;
				item.Content = "";
				//refresh - preopocitani poradi radku
				$scope.$emit("refreshgrid");
			});
	};

	var broadcastTime = function() {
		$scope.$broadcast("gridpage-tick", new Date() );
		timeout();
	};
	var timeout = function () {	setTimeout(broadcastTime, 60000 );};
	timeout();


	$scope.$on("gridelement-save", function (e, gridelement) {
		$scope.save(gridelement);
	});

	$scope.$on("gridname.showPreview", function (e, gridelemnt) {
		GridApi.updateGrid($scope.data);
	});

	$scope.save = function (item) {
		var copy = jQuery.extend(true, {}, item);
		gridEelementApi.put(copy);
	};

	$scope.showHelp = function(gridelement) {
		var help = gridelement.help || false;
		gridelement.help = !help;
	};
}];
