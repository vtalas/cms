/*global & */
var module = angular.module("gridsmodule", ["cmsapi", "templateExt", "ui", 'ui.bootstrap']);

function removeFromArray(item, collection) {
	var index;
	index = collection.indexOf(item);
	collection.splice(index, 1);
};

function pushToIndex(item, collection, index) {
	collection.splice(index, 0, item);
}

module.value('ui.config', {
	dropablehtml: {
		pageslist: {
			dragleave: function (e, uiConfig, element, xxx) {
				xxx.destinationScope.$emit("dragleave", xxx);
			},
			dragenter: function (e, uiConfig, element, xxx) {
				e.preventDefault();
				xxx.destinationScope.$emit("dragenter", xxx);
			},
			dragover: function (e, uioptions, element, xxx) {
				e.preventDefault();
				e.stopPropagation();
				xxx.destinationScope.$emit("dragover", xxx);
			},
			drop: function (e, uioptions, element, xxx) {
				var destCollection,
					item;

				destCollection = xxx.destinationScope.$parent.$collection;
				item = $.extend(true, {}, xxx.sourceItem);
				//console.log(xxx.sourceElement, xxx.destinationElement);

				this.pushToIndexOrLast(item, destCollection, xxx.destinationItem, uioptions.last);

				xxx.destinationScope.$emit("drop", xxx);
			},

			pushToIndexOrLast: function (item, collection, placeholderitem, islast) {
				var index;

				index = collection.indexOf(placeholderitem);
				if (placeholderitem === null || islast || index === -1) {
					collection.push(item);
				} else {
					collection.splice(index, 0, item);
				}
			}
		},
		sortable: {
			dragleave: function (e, uiConfig, element, xxx) {
				e.stopPropagation();
				xxx.destinationScope.$emit("dragleave", xxx);
			},
			dragenter: function (e, uiConfig, element, xxx) {
				e.preventDefault();
				xxx.destinationScope.$emit("dragenter", xxx);
			},
			dragover: function (e, uiConfig, element, xxx) {
				if (xxx.sourceItem === xxx.destinationItem) {
					return;
				}
				e.stopPropagation();
				e.preventDefault();
				xxx.destinationScope.$emit("dragover", xxx);
			},
			drop: function (e, uioptions, element, xxx) {
				var destCollection,
					item;
				destCollection = xxx.destinationScope.$parent.$collection;
				item = $.extend(true, {}, xxx.sourceItem);
				item.prdel = "xxxx";

				this.pushToIndexOrLast(item, destCollection, xxx.destinationItem, uioptions.last);
				xxx.sourceItem.status = DROPPED;

				xxx.destinationScope.$apply();
				xxx.destinationScope.$emit("drop", xxx);
			},
			pushToIndexOrLast: function (item, collection, placeholderitem, islast) {
				var index;

				index = collection.indexOf(placeholderitem);
				if (placeholderitem === null || islast || index === -1) {
					collection.push(item);
				} else {
					collection.splice(index, 0, item);
				}
			}
		}
	},
	draggablehtml: {
		pageslist: {
			dragstart: function (e, uioptions, element, xxx) {
				e.originalEvent.dataTransfer.effectAllowed = 'move';
				e.originalEvent.dataTransfer.setData('Text', xxx.sourceItem.Id);
				xxx.destinationScope.$emit("dragstart", xxx);
			}
		},
		sortable: {
			dragstart: function (e, uioptions, element, xxx) {
				var parent = $(xxx.sourceElement).data("id") !== $(e.target).data("id");

				if (parent) {
					//e.preventDefault();
					//	return;
				}
				e.stopPropagation();

				e.originalEvent.dataTransfer.effectAllowed = 'move';
				e.originalEvent.dataTransfer.setData('Text', xxx.sourceItem.Id);
				xxx.destinationScope.$emit("dragstart", xxx);
			},
			dragend: function (e, uiConfig, element, xxx) {
				if (xxx.sourceItem.status === DROPPED) {
					removeFromArray(xxx.sourceItem, xxx.sourceScope.$collection);
				}
				e.stopPropagation();
				xxx.destinationScope.$apply();
				xxx.destinationScope.$emit("dragend", xxx);
			}
		}
	},
	select2: {
		allowClear: true
	}
});

module.config(['$routeProvider', '$provide', function ($routeProvider, $provide) {
	$provide.factory('appSettings', function () {
		//var e = angular.element(".gridsmodule"),
		var e = $(".gridsmodule"),
			settings = {
				Name: e.data("application-name"),
				Id: e.data("application-id"),
				Culture: e.data("application-culture")
			};
		return settings;
	});
	$routeProvider
		.when('/gridpages', {controller: gridpagesCtrl, templateUrl: 'template/gridpages'})
		.when('/showuserdata', { controller: showuserdataCtrl, templateUrl: 'template/showuserdata' })
		.when('/applicationusers', { controller: applicationusersCtrl, templateUrl: 'template/applicationusers' })
		.when('/menu/:Id', {controller: menuCtrl, templateUrl: 'template/menu'})
		.when('/menu2/:Id', {controller: menu2Ctrl, templateUrl: 'template/menu2'})
		.when('/page/:Id', {controller: pageCtrl, templateUrl: 'template/gridpage'})
		.otherwise({ redirectTo: '/gridpages' });
}]);

module.controller("cultureCtrl", ["$scope", "appSettings", "GridApi", "$location", function ($scope, appSettings, GridApi, $location) {
	$scope.currentCulture = appSettings.Culture;
	$scope.setCulture = function (culture) {

		if (culture !== appSettings.Culture) {
			GridApi.setCulture(JSON.stringify(culture), function () {
				appSettings.Culture = culture;
				$scope.currentCulture = culture;
				$scope.$parent.$root.$broadcast("setCultureEvent");
			});
		}
	};
}]);

module.controller("chuj", ["$scope", "appSettings", "GridApi", "$location", function ($scope, appSettings, GridApi, $location) {
	$scope.section = "#" +$location.$$url;
	$scope.set = function (e) {
		$scope.section = e.srcElement.hash;
	};
	$scope.isActive = function (section) {
		return section === $scope.section ? "active" : "";
	};
}]);


module.directive("gridelement", ["$compile", "GridApi", "appSettings", "gridtemplate", function ($compile, GridApi, appSettings, gridtemplate) {

	return {
		scope: { grid: "=", gridelement: "=" },
		link: function (scope, iElement, tAttrs, controller) {
			scope.getGridElement = function (){
				return scope.gridelement;
			};

			scope.gui = { edit: 0 };

			var template = gridtemplate(scope.gridelement.Type + ".thtml"),
				compiled = $compile(template)(scope);
			iElement.html(compiled);
		}
	};
}]);


module.directive("ngcHover", function () {
	return {
		link: function (scope, element, attrs) {
			element.bind('mouseenter', function () {
				element.find(".hider").show();
			});
			element.bind('mouseleave', function () {
				element.find(".hider").hide();
			});
		}
	};
});

module.directive("ngcClickEdit", ngcClickEditDirective);
module.directive("ngcGdataGallery", ["gdataPhotos", ngcGDataGallery]);
//module.directive("ngcGdataGallery", ["gdataPhotos",  ngcGDataGallery ]);
module.directive("ngcLoader", ngcLoader);
module.directive("ngcLazyImage", function () {
	var loader = "/Content/loader16.gif";
	return {
		scope: {
			ngcLazyImage: "="
		},
		link: function (scope, element, attrs) {
			if (attrs.loader !== undefined ){
				loader = attrs.loader;
			}
			element.attr("src", loader);

			scope.$watch("ngcLazyImage", function (url, oldValue) {
				if (url !== undefined){
					element.attr("src", url);
				}
			});
		}
	}
});
