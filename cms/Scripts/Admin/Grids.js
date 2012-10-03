

var module = angular.module("gridsmodule", ["cmsapi", "templateExt", "ui"]);
var DRAGGED = 1,
	PRD = 0,
	DRAGEND = 5,
	DROPPED = 3,
	SWAPPED = 2;

function removeFromArray(item, collection) {
	var index;
	index = collection.indexOf(item);
	collection.splice(index, 1);
};

function pushToIndex(item, collection, index) {
	collection.splice(index, 0, item);
}


module.value('ui.config', {
	sortablehtml: {
		sortable: {
			dragstart: function (e, uioptions, element, xxx) {
				e.originalEvent.dataTransfer.effectAllowed = 'move';
				e.originalEvent.dataTransfer.setData('Text', xxx.source.item.Id);
				e.stopPropagation(); //pro pripad ze je sortable v sortable 

				xxx.source.item.status = DRAGGED;
				xxx.destination.scope.$emit("dragstart-sortablehtml", xxx);
			},
			dragend: function (e, uioptions, element, xxx) {
				xxx.source.item.status = DRAGEND;
				xxx.destination.scope.$apply();
				xxx.destination.scope.$emit("dragend-sortablehtml", xxx);
			},
			dragenter: function (e, uioptions, element, xxx) {
				e.stopPropagation(); //pro pripad ze je sortable v sortable 
				var destinationIsChild = (xxx.source.element).find(xxx.destination.element).length > 0;
				//console.log("enter", destinationIsChild);
				//console.log("enter", nestedSortable,  xxx.source.item.Id, xxx.destination.item.Id, xxx.source.element, xxx.destination.element);
				if (destinationIsChild) {
					return;
				}
				var swapItems = function (collection, sourceIndex, destinationIndex) {
					var tempSource = collection[sourceIndex];

					collection[sourceIndex] = collection[destinationIndex];
					collection[destinationIndex] = tempSource;
				};
				console.log(xxx.source.scope.$parent.$collection[0].Id, xxx.destination.scope.$parent.$collection[0].Id);
				var collectiondest = xxx.destination.scope.$parent.$collection,
					collectionsrc = xxx.source.scope.$parent.$collection,
					areInSameCollection,
					sourceindex = collectionsrc.indexOf(xxx.source.item),
					destinationindex = collectionsrc.indexOf(xxx.destination.item);

				if (xxx.source.item !== xxx.destination.item) {
					areInSameCollection = (destinationindex !== -1);

					if (areInSameCollection) {
						swapItems(collectionsrc, sourceindex, destinationindex);
					} else {
				//		console.log("xxxxxxxxxxxxxx notinSAME", xxx.source.scope.$parent.$collection.length, xxx.destination.scope.$parent.$collection.length);
						destinationindex = collectiondest.indexOf(xxx.destination.item);
						removeFromArray(xxx.source.item, collectionsrc);
						pushToIndex(xxx.source.item, collectiondest, destinationindex);
					}
					xxx.destination.item.status = SWAPPED;
					xxx.destination.scope.$apply();
				}
				xxx.source.item.status = DRAGGED;
				xxx.destination.scope.$emit("dragenter-sortablehtml", xxx);
			},
			dragleave: function (e, uioptions, element, xxx) {
				xxx.destination.item.status = PRD;
				xxx.source.item.status = DRAGGED;
				xxx.destination.scope.$apply();
				xxx.destination.scope.$emit("dragleave-sortablehtml", xxx);
			},
			dragover: function (e, uioptions, element, xxx) {
				e.preventDefault();
				e.stopPropagation();
				xxx.destination.scope.$emit("dragover-sortablehtml", xxx);
			},
			drop: function (e, uioptions, element, xxx) {
				xxx.source.item.status = DROPPED;
				xxx.destination.scope.$apply();
				xxx.destination.scope.$emit("drop-sortablehtml", xxx);
			}
		}
	},
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
		.when('/menu2/:Id', {controller: menu2Ctrl, templateUrl: 'template/menu2'})
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