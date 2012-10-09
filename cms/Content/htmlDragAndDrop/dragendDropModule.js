var module = angular.module("dragenddropapp", ["ui"]);

function removeFromArray(item, collection) {
	var index;
	if (!item) {
		return;
	}
	index = collection.indexOf(item);
	collection.splice(index, 1);
};

function pushToIndex(item, collection, index) {


	collection.splice(index, 0, item);
}

function findByStatus(collection, status) {
	var len = collection.length;
	for (var i = 0; i < len; i++) {
		if(collection[i].status === status) {
			return i;
		}
	}
	return -1;
}

function hideItem(item) {
	item.statusclass = "pseudohidden";
	item.status = PSEUDOHIDDEN;
}
function showItem(item, newstatus) {
	item.statusclass = "";
	item.status = newstatus;
}

function showOrPush(collectiondest, dndObject) {

	var destinationindex = collectiondest.indexOf(dndObject.destination.item);
	var clonedindexdest = findByStatus(collectiondest, PSEUDOHIDDEN);

	if (clonedindexdest !== -1) {
		showItem(collectiondest[clonedindexdest], DRAGGED);
		dndObject.pseudohidden.item = null;
		dndObject.source.item = collectiondest[clonedindexdest];
	} else {
		var clone = angular.extend({}, dndObject.source.item);
		clone.status = DRAGGED;
		clone.statusclass = "";
		clone.isClone = true;
		dndObject.source.item = clone;
		pushToIndex(clone, collectiondest, destinationindex);
	}
};


module.value('ui.config', {
	draganddrophtml: {
		sortable: {
			dragstart: function (e, uioptions,  dndobj) {
				e.originalEvent.dataTransfer.effectAllowed = 'move';
				e.originalEvent.dataTransfer.setData('Text', dndobj.source.item.Id);
				e.stopPropagation(); //pro pripad ze je sortable v sortable 

				dndobj.setSourceStatus(DRAGGED);
				dndobj.$emit("dragstart-sortablehtml");
			},
			dragend: function (e, uioptions, element, xxx) {
				console.log("end", xxx.source.item.status);
				xxx.setSourceStatus(DRAGEND);
				xxx.source.item.isClone = false;

				removeFromArray(xxx.pseudohidden.item, xxx.pseudohidden.collection);

				xxx.destination.scope.$apply();
				xxx.$emit("dragend-sortablehtml");
				e.stopPropagation(); //pro pripad ze je sortable v sortable 
			},
			dragenter: function (e, uioptions, element, xxx) {
				var swapItems,
					//removeOrHide,
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

				if (xxx.source.item !== xxx.destination.item) {
					areInSameCollection = (destinationindex !== -1);

					if (areInSameCollection) {
						swapItems(collectionsrc, sourceindex, destinationindex);
					} else {
						//remove or hide
						if (xxx.source.item.isClone) {
							removeFromArray(xxx.source.item, collectionsrc);
						} else {
							hideItem(xxx.source.item);
							
							xxx.pseudohidden.item = xxx.source.item;
							xxx.pseudohidden.collection = collectionsrc;
						}
						showOrPush(collectiondest, xxx);
						xxx.source.scope = xxx.destination.scope;
					}
				}
				xxx.destination.scope.$apply();
				xxx.$emit("dragenter-sortablehtml");
			},
			dragleave: function (e, uioptions, element, xxx) {
				xxx.destination.scope.$emit("dragleave-sortablehtml", xxx);
			},
			dragover: function (e, uioptions, element, xxx) {
				e.preventDefault();
				e.stopPropagation();
				xxx.$emit("dragover-sortablehtml");
			},
			drop: function (e, uioptions, element, xxx) {
				console.log("drop");
				xxx.setSourceStatus(DROPPED);
				xxx.destination.scope.$apply();
				xxx.$emit("drop-sortablehtml");
				e.stopPropagation(); //nested list 
			}
		}
	}
});