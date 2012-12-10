angular.module('ui.directives').value('ui.config.default', {
	draganddrophtml: {
		sortable: {
			dragstart: function (e, uioptions, dndobj) {
				e.originalEvent.dataTransfer.effectAllowed = 'move';
				e.originalEvent.dataTransfer.setData('Text', dndobj.source.item.Id);
				e.stopPropagation(); //pro pripad ze je sortable v sortable 

				dndobj.setSourceStatus(StatusEnum.DRAGGED);
				dndobj.$emit("dragstart-sortablehtml");
			},
			dragend: function (e, uioptions, dndobj) {
				console.log("end", dndobj.source.item);

				dndobj.dragEnd();

				dndobj.destination.scope.$apply();
				dndobj.$emit("dragend-sortablehtml");
				e.stopPropagation(); //pro pripad ze je sortable v sortable 
			},
			dragenter: function (e, uioptions, dndobj) {

				var destinationIsChildOfDragged = (dndobj.source.element).find(dndobj.destination.element).length > 0;
				e.stopPropagation(); //pro pripad ze je sortable v sortable 
				if (destinationIsChildOfDragged) {
					return;
				}

				if (dndobj.source.item !== dndobj.destination.item) {
					if (dndobj.sameCollection()) {
						dndobj.swapItems();
					} else {
						dndobj.changeParent();
					}
				}

				dndobj.destination.scope.$apply();
				dndobj.$emit("dragenter-sortablehtml");
			},
			dragleave: function (e, uioptions, dndobj) {
				dndobj.$emit("dragleave-sortablehtml", dndobj);
			},

			dragover: function (e, uioptions, dndobj) {
				e.preventDefault();
				e.stopPropagation();
				dndobj.$emit("dragover-sortablehtml");
			},

			drop: function (e, uioptions, dndobj) {
				console.log("drop");
				dndobj.setSourceStatus(StatusEnum.DROPPED);
				dndobj.destination.scope.$apply();
				dndobj.$emit("drop-sortablehtml");
				e.stopPropagation(); //nested list 
			}
		}
	}
});

angular.module('ui.directives').directive('uiDraganddropHtml', ['ui.config', 'ui.config.default', function (uiConfig, uiConfigDefault) {
	var options = {},
		dndobj = new DragAndDropObjts();

	if (uiConfig.draganddrophtml === undefined) {
		angular.extend(options, uiConfigDefault.draganddrophtml);
	}

	return {
		require: '?ngModel',
		link: function (scope, element, attrs, ngModel) {
			var namespaceArray = attrs.uiDraganddropHtml.split(","),
			    namespace = namespaceArray[0],
				opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

			(element).attr("draggable", true);

			scope.$watch("item.status", function (newval, oldval) {
				scope.$emit("statuschange-sortablehtml", { oldvalue: oldval, newvalue: newval, element: element, item: ngModel.$modelValue });
			});

			element.on("dragstart", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				var sourceitem = new DndItem(modelvalues, scope, element, namespace);
				dndobj.load(sourceitem, sourceitem);

				if (typeof (opts[namespace].dragstart) === "function") {
					opts[namespace].dragstart(e, options, dndobj);
				}
			});

			element.on("dragenter", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].dragenter) === "function") {
					dndobj.destination = new DndItem(modelvalues, scope, element);
					opts[namespace].dragenter(e, options, dndobj);
				}
			});
			element.on("dragleave", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;
				if (typeof (opts[namespace].dragleave) === "function") {
					dndobj.destination = new DndItem(modelvalues, scope, element);
					opts[namespace].dragleave(e, options, dndobj);
				}
			});
			element.on("dragover", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].dragover) === "function") {
					dndobj.destination = new DndItem(modelvalues, scope, element);
					opts[namespace].dragover(e, options, dndobj);
				}
			});
			element.on("drop", function (e) {
				var modelvalues = ngModel ? ngModel.$modelValue : null;

				if (typeof (opts[namespace].drop) === "function") {
					dndobj.destination = new DndItem(modelvalues, scope, element);
					opts[namespace].drop(e, options, dndobj);
				}
			});
			//dragend nevi o koncovym elementu, proto se destination neupdatuje
			element.on("dragend", function (e) {
				if (typeof (opts[namespace].dragend) === "function") {
					opts[namespace].dragend(e, options, dndobj);
				}
			});



		}
	};
}
]);
















var xxx = function (source, destination, namespace) {
	return {
		sourceItem: source.item,
		sourceScope: source.scope,
		sourceElement: source.element,
		destinationItem: destination.item,
		destinationScope: destination.scope,
		destinationElement: destination.element,
		namespace: namespace
	};
};

function getNamespace(scope, namespaceArray) {
	var validNamespaceIndex, validNamespace;

	if (scope.$root.draggeditem) {
		validNamespaceIndex = namespaceArray.indexOf(scope.$root.draggeditem.namespace);
		validNamespace = namespaceArray[validNamespaceIndex];
	}
	return validNamespace;
}

function registerDragEventWithNamespace(dragevent, scope, namespaceArray, element, ngModel, opts) {
	$(element).on(dragevent, function (e) {

		var namespace = getNamespace(scope, namespaceArray),
			obj,
		    source,
		    destination,
			modelvalues = ngModel ? ngModel.$modelValue : null;
		if (!opts[namespace]) {
			console.log("namespace is not defined", opts[namespace]);
			return;
		}
		if (typeof (opts[namespace][dragevent]) === "function") {
			source = { item: scope.$root.draggeditem, scope: scope.$root.draggedScope, element: scope.$root.draggedElement };
			destination = { item: modelvalues, scope: scope, element: element };
			obj = xxx(source, destination, namespace);
			opts[namespace][dragevent](e, opts, element, obj);
		}
	});

}

angular.module('ui.directives').directive('uiDraggableHtml', [
	'ui.config', function (uiConfig) {
		var options;
		options = {};
		if (uiConfig.draggablehtml !== null) {
			angular.extend(options, uiConfig.draggablehtml);
		}
		return {
			require: '?ngModel',

			link: function (scope, element, attrs, ngModel) {
				var namespace = attrs.uiDraggableHtml,
					opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				$(element).attr("draggable", true);

				$(element).on("dragstart", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null,
					    source, destination;

					scope.$root.draggeditem = modelvalues;
					scope.$root.draggedScope = scope;
					scope.$root.draggedElement = element;
					scope.$root.draggeditem.namespace = namespace;

					if (typeof (opts[namespace].dragstart) === "function") {
						source = { item: scope.$root.draggeditem, scope: scope.$root.draggedScope, element: scope.$root.draggedElement };
						destination = { item: modelvalues, scope: scope, element: element };
						opts[namespace].dragstart(e, options, element, xxx(source, destination, namespace));
					}
				});
				$(element).on("dragend", function (e) {
					var modelvalues = ngModel ? ngModel.$modelValue : null,
					    source, destination;
					if (typeof (opts[namespace].dragend) === "function") {
						source = { item: scope.$root.draggeditem, scope: scope.$root.draggedScope, element: scope.$root.draggedElement };
						destination = { item: modelvalues, scope: scope, element: element };
						opts[namespace].dragend(e, options, element, xxx(source, destination, namespace));
					}
				});
			}
		};
	}
]);
angular.module('ui.directives').directive('uiDropableHtml', [
	'ui.config', function (uiConfig) {
		var options;
		options = {};
		if (uiConfig.dropablehtml !== null) {
			angular.extend(options, uiConfig.dropablehtml);
		}

		return {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModel) {
				var namespaceArray = attrs.uiDropableHtml.split(","),
					opts = angular.extend({}, options, scope.$eval(attrs.uiOptions));

				registerDragEventWithNamespace("dragenter", scope, namespaceArray, element, ngModel, opts);
				registerDragEventWithNamespace("dragleave", scope, namespaceArray, element, ngModel, opts);
				registerDragEventWithNamespace("dragover", scope, namespaceArray, element, ngModel, opts);
				registerDragEventWithNamespace("drop", scope, namespaceArray, element, ngModel, opts);
			}
		};
	}
]);