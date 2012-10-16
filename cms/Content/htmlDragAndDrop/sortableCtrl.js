var sortableCtrl = function ($scope, $http) {
	"use strict";
	sortableCtrl.$inject = ["$scope", "$http"];

	$scope.$on("statuschange-sortablehtml", function (data, xxx) {
		var old = (xxx.element).css("background-color");
		switch (xxx.newvalue) {
		case StatusEnum.DRAGEND:
			(xxx.element).css("background-color", "green");
			$(".sortableCtrl").stop(true, true).css("opacity", "1");
			(xxx.element).animate({ backgroundColor: old }, 1000);
			console.log("xxxx", xxx.element);
			break;
		case StatusEnum.DRAGGED:
			(xxx.element).css("opacity", 1);
			break;
		case StatusEnum.DROPPED:
			break;
		case StatusEnum.SWAPPED:
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

	$scope.add = function (item) {
		console.log("add item", item);
	};

	$scope.getGrid = function () {
		$http({ method: 'GET', url: '/Content/htmlDragAndDrop/sortableData.json' }).
			success(function (data, status, headers, config) {
				$scope.data = data;
			});
		return;
	};

	$scope.getGrid();

};

var sortableNestedCtrl = function ($scope, $http) {
	"use strict";
	sortableNestedCtrl.$inject = ["$scope", "$http"];

	$scope.$on("statuschange-sortablehtml", function (data, xxx) {
		switch (xxx.newvalue) {
		case StatusEnum.DRAGEND:
			(xxx.element).css("opacity", 1);
			console.log("xx", xxx.element);
			break;
		case StatusEnum.DRAGGED:
			(xxx.element).css("opacity", 0.5);
			break;
		case StatusEnum.DROPPED:
			break;
		case StatusEnum.DRAGOLD:
			console.log("dragold");
			//xxx.element.css("border", "2px solid red");
			break;
		case StatusEnum.SWAPPED:
	//		(xxx.element).css("opacity", 1);
			setTimeout(function () {
//				(xxx.element).animate({ opacity: 0.4 }, 1000);
			}, 200);
			break;
		default:
		}
	});

	$scope.$on("dragstart-sortablehtmlPico", function (data, dndObj) {
		console.log(dndObj)
		//$(".sortableNestedCtrl").css("opacity", "0.4");
	});
	$scope.$on("dragend-sortablehtml", function (data, xxx) {
		//$(".sortableNestedCtrl").stop(true).css("opacity", "1");
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

	$scope.json = function () {
		return JSON.stringify($scope.data);
	};

	$scope.add = function (item) {
		console.log("add item", item);
	};

	$scope.getGrid = function () {
		$http({ method: 'GET', url: '/Content/htmlDragAndDrop/sortableNestedData.json' }).
			success(function (data, status, headers, config) {
				$scope.data = data;
				});
		return;
	};

	$scope.getGrid();

};