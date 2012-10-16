var menuCtrl = function ($scope, $http, $rootScope, appSettings, apimenu, $routeParams, GridApi) {
	menuCtrl.$inject = ["$scope", "$http", "apimenu", "appSettings", "$routeParams"];

	$scope.$on("statuschange-sortablehtml", function (data, xxx) {
		var old = (xxx.element).css("background-color");
		switch (xxx.newvalue) {
		case StatusEnum.DRAGEND:
			(xxx.element).css("background-color", "green");
			$(".sortableCtrl").stop(true, true).css("opacity", "1");
			(xxx.element).animate({ backgroundColor: old }, 1000);
			break;
		case StatusEnum.DRAGGED:
			(xxx.element).css("opacity", 1);
			break;
		case StatusEnum.DROPPED:
			break;
		case StatusEnum.SWAPPED:
			console.log("swapped")
			(xxx.element).css("background-color", "green");
			setTimeout(function () {
				(xxx.element).animate({ backgroundColor: old }, 1000);
			}, 200);
			break;
		default:
		}
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
	$scope.grids = GridApi.grids({ }, function (data) {

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

	$scope.$on("setCultureEvent", function () {
		console.log("menuCtrl set culture");
	});

	$scope.$on("statuschange-sortablehtml", function (data, xxx) {
		var old = (xxx.element).css("background-color");
		switch (xxx.newvalue) {
		case DRAGEND:
			(xxx.element).css("background-color", "green");
			$(".sortableCtrl").stop(true, true).css("opacity", "1");
			(xxx.element).animate({ backgroundColor: old }, 1000);
			break;
		case DRAGGED:
			(xxx.element).css("opacity", 1);
			break;
		case DROPPED:
			break;
		case SWAPPED:
			(xxx.element).css("opacity", 1);
			setTimeout(function () {
				(xxx.element).animate({ opacity: 0.4 }, 1000);
			}, 200);
			break;
		default:
		}
	});
	$scope.$on("dragstart-sortablehtml", function (data, xxx) {
		$(".sortableCtrl").css("opacity", "0.4");
	});
	$scope.$on("dragend-sortablehtml", function (data, xxx) {
		//$(".sortableCtrl").stop(true).css("opacity", "1");
	});
	$scope.$on("dragenter-sortablehtml", function (data, xxx) {
		//	console.log("enter", xxx.source.element, xxx.destination.element);
	});
	$scope.$on("dragleave-sortablehtml", function (data, xxx) {
		//console.log("leave", xxx.destination.item.status, xxx.source.item.status);
	});
	$scope.$on("dragover-sortablehtml", function (data, xxx) {
		//console.log("over");
	});
	$scope.$on("drop-sortablehtml", function (data, xxx) {
		//console.log("drop", [xxx.destination.item.Id], xxx.destination.item);
	});


	$scope.showAdd = function (item) {
		item.showAdd = 1;
	};
	$scope.hideAdd = function (item) {
		item.showAdd = 0;
	};
	$scope.grids = GridApi.grids({ }, function (data) {

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