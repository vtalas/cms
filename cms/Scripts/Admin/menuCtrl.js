var menusCtrl = function ($scope, $http, $rootScope, appSettings, apimenu, GridApi) {
	menusCtrl.$inject = ["$scope", "$http", "apimenu", "appSettings"];

	$scope.$on("setCultureEvent", function () {
		console.log("menuCtrl set culture");
	});

	$scope.data = apimenu.list({ }, function (d) {
		console.log(d);
	});
	$scope.addclick = function () {
		$scope.createNew = true;
	};
	$scope.createNewCancel = function () {
		$scope.createNew = false;
	};
};

var menuCtrl = function ($scope, $http, $rootScope, appSettings, apimenu, $routeParams, GridApi) {
	menuCtrl.$inject = ["$scope", "$http", "apimenu", "appSettings", "$routeParams"];

	$scope.dragging = false;

	$scope.$on("setCultureEvent", function () {
		console.log("menuCtrl set culture");
	});

	$scope.$on("dragstart", function (data, xxx) {
		$(xxx.sourceElement).addClass("dragged");
		setTimeout(function () {
			$(".dropableCtrl").animate({ height: 20 }, 200);	
		}, 200);
	});

	$scope.$on("dragover", function (data, xxx) {
	});

	$scope.$on("dragenter", function (data, xxx) {
		$(xxx.destinationElement).addClass("dragover");
	});

	$scope.$on("dragleave", function (data, xxx) {
		$(xxx.destinationElement).removeClass("dragover");
	});

	$scope.$on("drop", function (data, xxx) {
		$scope.dragendAction(xxx);
		//$scope.add(item);
	});

	$scope.$on("dragend", function (data, xxx) {
		$scope.dragendAction(xxx);
	});


	$scope.dragendAction = function (xxx) {
		$(".dropableCtrl").removeClass("dragover");
		$(xxx.destinationElement).css("background-color", "#eee");
		$(xxx.destinationElement).animate({ backgroundColor: "#fff" }, 1000);
		$(xxx.destinationElement).removeClass("dragged");
		$(".dropableCtrl").animate({ height: 0 }, 100);
	};

	$scope.showAdd = function (item) {
		item.showAdd = 1;
	};
	$scope.hideAdd = function (item) {
		item.showAdd = 0;
	};
	$scope.grids = GridApi.grids({}, function (data) {

	});
	$scope.add = function (item) {
		console.log("add", item);
		var menuitem = {
			Line: 0,
			ParentId: item.Id,
			Type: "gridpagereference"
		};

	};


	var params = { id: $routeParams.Id };

	$scope.getGrid = function () {
		$http({ method: 'GET', url: '/content/test/menu2.json' }).
			success(function (data, status, headers, config) {
				$scope.data = data;
			});
		return;
		apimenu.get(params, function (data) {
			$scope.data = data;
			console.log(data);
			//var newitem = _newitem($scope.data.Lines.length);
			//$scope.data.Lines.push([newitem]);
		});
	};

	$scope.getGrid();


};

var menu2Ctrl = function ($scope, $http, $rootScope, appSettings, apimenu, $routeParams, GridApi) {
	menu2Ctrl.$inject = ["$scope", "$http", "apimenu", "appSettings", "$routeParams"];

	$scope.dragging = false;


	var timeout,
		dropableStartWidth = 10,
		draggableWidth = 170;
	$scope.$on("setCultureEvent", function () {
		console.log("menuCtrl set culture");
	});

	$scope.$on("statuschange", function (data, xxx) {
		var old = (xxx.element).css("background-color");
		(xxx.element).css("background-color", xxx.newvalue);
		(xxx.element).animate({ backgroundColor: old }, 1000);
	});
	$scope.$on("dragstart-sortablehtml", function (data, xxx) {
		$(xxx.source.element).addClass("dragged");
		console.log("start", xxx, $(xxx.source.element));
	});
	$scope.$on("dragend-sortablehtml", function (data, xxx) {
		console.log("end", [xxx.destination.item.Id], xxx.destination.item);
	});
	$scope.$on("dragenter-sortablehtml", function (data, xxx) {
		(xxx.source.element).css("color", "red");
		console.log("enter", xxx.source.element, xxx.destination.element);
	});
	$scope.$on("dragleave-sortablehtml", function (data, xxx) {
		console.log("leave", [xxx.destination.item.Id], xxx.destination.item);
	});
	$scope.$on("dragover-sortablehtml", function (data, xxx) {
		console.log("over");
	});
	$scope.$on("drop-sortablehtml", function (data, xxx) {
		console.log("drop", [xxx.destination.item.Id], xxx.destination.item);
	});



	$scope.$on("dragstart", function (data, xxx) {
		$(xxx.sourceElement).addClass("dragged");
		setTimeout(function () {
			$(xxx.sourceElement).animate({ width: 0 });
			$(xxx.sourceElement).hide();
			//$(".dropableCtrl").animate({ width: 20 }, 200);
			//$(".draggableCtrl").animate({ width: 130 }, 200);
		}, 200);
	});

	$scope.$on("dragover", function (data, xxx) {
		if (!$(xxx.destinationElement).hasClass("dragover")) {
			$(xxx.destinationElement).addClass("dragover");
		}
	});

	$scope.$on("dragenter", function (data, xxx) {
		$(xxx.destinationElement).addClass("dragover");
		timeout = setTimeout(function () {
			//$(xxx.destinationElement).animate({ width: draggableWidth + 2 * dropableStartWidth }, 200);
		}, 500);

	});

	$scope.$on("dragleave", function (data, xxx) {
		$(xxx.destinationElement).removeClass("dragover");
		clearTimeout(timeout);
		$(xxx.destinationElement).animate({ width: dropableStartWidth }, 100);
	});

	$scope.$on("drop", function (data, xxx) {
		//$scope.add(item);
	});

	$scope.$on("dragend", function (data, xxx) {
		$scope.dragendAction(xxx);
	});


	$scope.dragendAction = function (xxx) {

		clearTimeout(timeout);

		$(".dropableCtrl").removeClass("dragover");
		$(".dropableCtrl").animate({ width: dropableStartWidth }, 200);
		$(xxx.destinationElement).css("background-color", "red");

		$(xxx.destinationElement).animate({ backgroundColor: "#aaa" }, 1000);
		$(xxx.destinationElement).removeClass("dragged");
		$(xxx.destinationElement).show();

		setTimeout(function () {
			//$(".draggableCtrl").animate({ width: 140}, 200);
		}, 500);


	};

	$scope.showAdd = function (item) {
		item.showAdd = 1;
	};
	$scope.hideAdd = function (item) {
		item.showAdd = 0;
	};
	$scope.grids = GridApi.grids({}, function (data) {

	});
	$scope.add = function (item) {
		console.log("add", item);
		var menuitem = {
			Line: 0,
			ParentId: item.Id,
			Type: "gridpagereference"
		};

	};


	var params = { id: $routeParams.Id };

	$scope.getGrid = function () {
		$http({ method: 'GET', url: '/content/test/menu2.json' }).
			success(function (data, status, headers, config) {
				$scope.data = data;
			});
		return;
		apimenu.get(params, function (data) {
			$scope.data = data;
			console.log(data);
			//var newitem = _newitem($scope.data.Lines.length);
			//$scope.data.Lines.push([newitem]);
		});
	};

	$scope.getGrid();


};