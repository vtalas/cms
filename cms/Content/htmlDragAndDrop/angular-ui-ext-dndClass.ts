 enum StatusEnum {
	DRAGGED =  1,
	DRAGEND = 5,
	DRAGOLD =  9,
	DROPPED =  3,
	PSEUDOHIDDEN = 333,
	SWAPPED = 2
}
 interface IAngularObject {
 	$$hashKey;
 }

interface IDragElement extends IAngularObject {
	isClone: bool;
	status: StatusEnum;
}

class DndItem {
	item: IDragElement;
	scope = {};
	element = {};
	namespace: string;

	constructor (item, scope, element, namespace) {
		this.item = item;
		this.scope = scope,
		this.element = element,
		this.namespace = namespace;
	}
	collection() {
		return this.scope.$parent.$collection;
	}
	hasCollection() {
		return (this.scope && this.scope.$parent && this.scope.$parent.$collection);
	}
	isClone() {
		return this.item.isClone;
	}
	removeFromCollection() {
		var index;
		if (!this.item || !this.hasCollection()) {
			return -1;
		}
		index = this.findById(this.item);
		this.collection().splice(index, 1);
	}
	findById(item:IAngularObject) {
		return this.findBy("$$hashKey", item.$$hashKey);
	}
	findBy(key, value) {
		var collection = this.collection();
		var len = collection.length;
		for (var i = 0; i < len; i++) {
			if(collection[i][key] === value) {
				return i;
			}
		}
		return -1;
	}
}

class DragAndDropObjts {
	source: DndItem;
	destination: DndItem;
	pseudohidden: DndItem;

	load(sourceItem: DndItem, destinationItem: DndItem) {
		this.source = sourceItem;
		this.destination = destinationItem;
		this.pseudohidden = new DndItem({}, {}, {}, "");
	}
	swapItems() {
		var collection = this.source.collection(),
			sourceindex = this.source.findById(this.source.item),
			destinationindex = this.source.findById(this.destination.item);

		var tempSource = collection[sourceindex];
		collection[sourceindex] = collection[destinationindex];
		collection[destinationindex] = tempSource;
	}
	removeOrHide() {
		if (this.source.isClone()) {
			this.source.removeFromCollection();
		} else {
			this.hide(this.source.item);
			this.pseudohidden.item = this.source.item;
			this.pseudohidden.scope = this.source.scope;
		}
	}
	hide(itemToHide) {
		itemToHide.statusclass = "pseudohidden";
		itemToHide.status = StatusEnum.PSEUDOHIDDEN;
	}
	show(itemToHide, newstatus:StatusEnum) {
		itemToHide.statusclass = "";
		itemToHide.status = newstatus;
	}
	showOrPush() {
		var collectiondest = this.destination.collection();
		var destinationindex = this.destination.findById(this.destination.item);
		var clonedindexdest = this.destination.findBy("status", StatusEnum.PSEUDOHIDDEN);

		if (clonedindexdest !== -1) {
			this.show(collectiondest[clonedindexdest], StatusEnum.DRAGGED);
			this.pseudohidden.item = null;
			this.source.item = collectiondest[clonedindexdest];
		} else {
			var clone = angular.extend({}, this.source.item);
			clone.status = StatusEnum.DRAGGED;
			clone.statusclass = "";
			clone.isClone = true;
			this.source.item = clone;
			this.pushToIndex(clone, collectiondest, destinationindex);
		}
	};
	pushToIndex(item, collection, index) {
		collection.splice(index, 0, item);
	}
	sameCollection() {
		return this.source.findById(this.destination.item) !== -1;
	}
	setSourceStatus(newstatus) {
		this.source.item.status = newstatus
	}
	cleanUp() {
		this.pseudohidden.removeFromCollection();
	}
	$emit(eventName) {
		this.source.scope.$emit(eventName, this);
	}
}