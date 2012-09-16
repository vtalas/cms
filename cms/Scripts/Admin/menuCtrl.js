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

	$scope.$on("dragover", function (data, item) {
		//console.log("dragover-menujs", item);
	});

	$scope.$on("dragenter", function (data, xxx) {
		$(xxx.destinationElement).addClass("dragover");
	});

	$scope.$on("dragleave", function (data, xxx) {
		$(xxx.destinationElement).removeClass("dragover");
	});

	$scope.$on("drop", function (data, xxx) {
		console.log("drop", xxx.destinationElement);
		//$scope.dragendAction(xxx);
		//$scope.add(item);
	});

	$scope.$on("dragend", function (data, xxx) {
		console.log("dragend",xxx.destinationElement)
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