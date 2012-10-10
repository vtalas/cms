var module = angular.module("gridsmodule", ["cmsapi", "templateExt", "ui"]);

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
		.when('/gridpages', {controller: gridpagesCtrl, templateUrl: 'template/gridpages'})
		.when('/menu/:Id', {controller: menuCtrl, templateUrl: 'template/menu'})
		.when('/menu2/:Id', {controller: menu2Ctrl, templateUrl: 'template/menu2'})
		.when('/page/:Id', {controller: pageCtrl, templateUrl: 'template/gridpage'})
		.otherwise({redirectTo: '/gridpages'});
}]);

module.controller("cultureCtrl", function ($scope, appSettings, GridApi) {
	$scope.currentCulture = appSettings.Culture;
	$scope.setCulture = function (culture) {

		if (culture !== appSettings.Culture) {
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

var gridElementCtrl = function ($scope, GridApi, appSettings) {
	$scope.addWithType = function (item, newtype, event) {
		event.preventDefault();
		item.Type = newtype;
		$scope.add(item);
	};
	$scope.add = function (item) {
		GridApi.AddGridElement({
			applicationId: appSettings.Id,
			data: item,
			gridId: $scope.grid.Id
		}, function (data) {
			if (data.Line >= $scope.grid.Lines.length - 1) {
				var newitem = _newitem($scope.grid.Lines.length, item.Type);
				$scope.grid.Lines.push([newitem]);
			}
			$scope.grid.Lines[data.Line][data.Position] = data;
			//TODO: nevyvola se broadcast
			$scope.edit(data);
		});
	};

	$scope.remove = function (item, gridId) {
		var line = $scope.$parent.$parent.line;
		GridApi.DeleteGridElement({ applicationId: appSettings.Id, data: item, gridId: gridId },
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
		if (item.Id !== 0) {
			item.Edit = 1;
		}
	};

	$scope.save = function (item) {
		var copy = jQuery.extend(true, {}, item);

		if (angular.isObject(copy.Content)) {
			copy.Content = JSON.stringify(copy.Content);
		}
		GridApi.UpdateGridElement({ applicationId: appSettings.Id, data: copy },
			function () {
				item.Edit = 0;
			});
	};

};

//module.directive("gridelement", ['$compile', "GridApi", "appSettings","$http",
module.directive("gridelement", function ($compile, GridApi, appSettings, gridtemplate, $templateCache) {

	function _newitem(line, type) {
		var newitem = {Id: 0, Width: 12, Type: type, Line: line, Edit: 0};
		return newitem;
	}

	var directiveDefinitionObject


	directiveDefinitionObject = {
		scope: { grid: "=", gridelement: "=" },
		controller: gridElementCtrl,
		//templateUrl: "/templates/tmpl",
		link: function (scope, iElement, tAttrs, controller) {
			scope.gui = { edit: 0 };
			var adminTemplate = gridtemplate(scope.gridelement.Type + "_admin.thtml"),
			    clientTemplate = gridtemplate(scope.gridelement.Type + ".thtml"),
			    adminCompiled = $compile(adminTemplate)(scope),
			    clientCompiled = $compile(clientTemplate)(scope);
			    
			iElement.find(".contentAdmin").html(adminCompiled);
			iElement.find(".contentClient").html(clientCompiled);
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