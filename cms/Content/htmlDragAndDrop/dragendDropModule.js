var module = angular.module("dragenddropapp", ["ui"]);

function removeFromArray(item, collection) {
	var index;
	index = collection.indexOf(item);
	collection.splice(index, 1);
};

function pushToIndex(item, collection, index) {
	collection.splice(index, 0, item);
}

function findByStatusClass(collection, statusclass) {
	var len = collection.length;
	for (var i = 0; i < len; i++) {
		if(collection[i].statusclass === statusclass) {
			return i;
		}
	}
	return -1;
}
function hideItem(item) {
	item.statusclass = "pseudohidden";
}

module.value('ui.config', {
	draganddrophtml: {
		sortable: {
			dragstart: function (e, uioptions, xxx) {
				e.originalEvent.dataTransfer.effectAllowed = 'move';
				e.originalEvent.dataTransfer.setData('Text', xxx.source.item.Id);
				e.stopPropagation(); //pro pripad ze je sortable v sortable 

				xxx.source.item.status = DRAGGED;
				xxx.destination.scope.$emit("dragstart-sortablehtml", xxx);
			},
			dragend: function (e, uioptions, element, xxx) {
				console.log("end", xxx.source.item.status);
				xxx.source.item.status = DRAGEND;
				xxx.destination.scope.$apply();
				xxx.destination.scope.$emit("dragend-sortablehtml", xxx);
			},
			dragenter: function (e, uioptions, element, xxx) {
				var swapItems,
				    areInSameCollection,
				    collectiondest = xxx.destination.scope.$parent.$collection,
				    collectionsrc = xxx.source.scope.$parent.$collection,
				    sourceindex,
				    destinationindex,
				    destinationIsChild = (xxx.source.element).find(xxx.destination.element).length > 0;

				e.stopPropagation(); //pro pripad ze je sortable v sortable 
				if (destinationIsChild) {
					return;
				}
				swapItems = function (collection, sourceIndex, destinationIndex) {
					var tempSource = collection[sourceIndex];
					collection[sourceIndex] = collection[destinationIndex];
					collection[destinationIndex] = tempSource;
				};

				sourceindex = collectionsrc.indexOf(xxx.source.item);
				destinationindex = collectionsrc.indexOf(xxx.destination.item);

				///console.log(x.target, x.dataTransfer, x.dataTransfer.items, x.dataTransfer.items.item());
				if (xxx.source.item !== xxx.destination.item) {
					areInSameCollection = (destinationindex !== -1);

					if (areInSameCollection) {
						swapItems(collectionsrc, sourceindex, destinationindex);
					} else {
						destinationindex = collectiondest.indexOf(xxx.destination.item);
						var clone = angular.extend({ }, xxx.source.item);
						clone.status = CLONE;

						pushToIndex(clone, collectiondest, destinationindex);

						//remove or hide
						if (xxx.source.item.status === CLONE) {
							removeFromArray(xxx.source.item, collectionsrc);
						} else {
							hideItem(xxx.source.item);
						}
						xxx.source.scope = xxx.destination.scope;
						xxx.source.item = clone;
					}
				}
				//console.log($(xxx.source.element));
				//xxx.source.item.status = DRAGGED;
				xxx.destination.scope.$emit("dragenter-sortablehtml", xxx);
			},
			dragleave: function (e, uioptions, element, xxx) {
				//xxx.destination.item.status = PRD;
				//xxx.source.item.status = DRAGGED;
				xxx.destination.scope.$apply();
				xxx.destination.scope.$emit("dragleave-sortablehtml", xxx);
			},
			dragover: function (e, uioptions, element, xxx) {
				e.preventDefault();
				e.stopPropagation();
				xxx.destination.scope.$emit("dragover-sortablehtml", xxx);
			},
			drop: function (e, uioptions, element, xxx) {
				console.log("drop" );
				xxx.source.item.status = DROPPED;
				xxx.destination.scope.$apply();
				xxx.destination.scope.$emit("drop-sortablehtml", xxx);
				e.stopPropagation(); //nested list 
			}
		}
	}
});