

var module = angular.module("gridsmodule", ["cmsapi", "templateExt", "ui"]);
var DROPPED = 0;

function removeFromArray(item, collection) {
	var index;
	index = collection.indexOf(item);
	collection.splice(index, 1);
};


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
				//console.log("dragenter", xxx.sourceItem.Id, xxx.destinationItem.Id);
				e.preventDefault();
				xxx.destinationScope.$emit("dragenter", xxx);
			},
			dragover: function (e, uiConfig, element, xxx) {
				//console.log("dragover", xxx.sourceItem.Id, xxx.destinationItem.Id);
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
				//this.removeSource(xxx.sourceItem, xxx.sourceScope.$parent.$collection);

				//console.log("drop,", xxx.sourceItem.Id);
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
				//console.log("dragstart", e, $(xxx.sourceElement).data("id") == $(e.target).data("id"), xxx.sourceElement);

				xxx.destinationScope.$emit("dragstart", xxx);
			},
			dragend: function (e, uiConfig, element, xxx) {
				//console.log("dragendd...", xxx.sourceItem.Id, xxx.destinationItem.Id);
				if (xxx.sourceItem.status === DROPPED) {
					removeFromArray(xxx.sourceItem, xxx.sourceScope.$collection);
				}
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
		var e = angular.element(".gridsmodule"),
			settings = {
				Name: e.data("application-name"),
				Id: e.data("application-id"),
				Culture: e.data("application-culture")
			};
		return settings;
	});
	$provide.constant();
	$routeProvider
		.when('/gridpages', {controller: GridListController, templateUrl: 'template/gridpages'})
		.when('/menus', {controller: menusCtrl, templateUrl: 'template/menus'})
		.when('/menu/:Id', {controller: menuCtrl, templateUrl: 'template/menu'})
		.when('/gridpage/:Id', {controller: GridPageCtrl, templateUrl: 'template/gridpage'})
		.otherwise({redirectTo: '/gridpages'});
}]);

module.controller("cultureCtrl", function ($scope, appSettings, GridApi) {
	$scope.currentCulture = appSettings.Culture;
	$scope.setCulture = function (culture) {

		if (culture != appSettings.Culture) {
			GridApi.setCulture({
					culture: culture
				}, function () {
					appSettings.Culture = culture;
					$scope.currentCulture = culture;
					$scope.$parent.$root.$broadcast("setCultureEvent");
				});
		}
	};
});

//module.directive("gridelement", ['$compile', "GridApi", "appSettings","$http",
module.directive("gridelement", function ($compile, GridApi, appSettings, gridtemplate) {

	function _newitem(line) {
		var newitem = {Id: 0, Width: 12, Type: "text", Line: line, Edit: 0};
		return newitem;
	}

	var directiveDefinitionObject,
		gridElementCtrl = function ($scope) {
			$scope.add = function (item) {
				GridApi.AddGridElement({
						applicationId: appSettings.Id,
						data: item,
						gridId: $scope.grid.Id
					}, function (data) {
						if (data.Line >= $scope.grid.Lines.length - 1) {
							var newitem = _newitem($scope.grid.Lines.length);
							$scope.grid.Lines.push([newitem]);
						}
						$scope.grid.Lines[data.Line][data.Position] = data;
						//TODO: nevyvola se broadcast
						$scope.edit(data);
					});
			};

			$scope.remove = function (item) {
				var line = $scope.$parent.$parent.line;

				GridApi.DeleteGridElement({applicationId: appSettings.Id, data: item, gridId: $scope.grid.Id},
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
				item.Edit = 1;
			};

			$scope.save = function (item) {
				var copy = jQuery.extend(true, {}, item);

				if (angular.isObject(copy.Content)) {
					copy.Content = JSON.stringify(copy.Content);
				}

				GridApi.UpdateGridElement({applicationId: appSettings.Id, data: copy},
					function () {
						item.Edit = 0;
					});
			};

		};

	directiveDefinitionObject = {
		scope: {grid: "=", gridelement: "="},
		controller: gridElementCtrl,
		link: function (scope, iElement, tAttrs, controller) {
			scope.gui = {edit: 0};
			var sablona = gridtemplate(scope.gridelement.Type),
				compiled = $compile(sablona)(scope);
			iElement.html(compiled);
		}
	};
	return directiveDefinitionObject;
});

module.directive("menuitem", function ($compile, GridApi, appSettings, menuItemTemplate) {
	var menuItemCtrl = function ($scope) {
		$scope.add = function (item) {
		};
		$scope.remove = function (item) {
		};
	};

	return {
		scope: {grid: "=", menuitem: "="},
		controller: menuItemCtrl,
		link: function (scope, iElement, tAttrs, controller) {
			scope.gui = {edit: 0};
			var sablona = menuItemTemplate(scope.menuitem.Type),
				compiled = $compile(sablona)(scope);
			iElement.html(compiled);
		}
	};
});