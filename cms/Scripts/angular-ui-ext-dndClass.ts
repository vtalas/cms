class DndItem {
    item = {};
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

}

class DragAndDropObjts {
	source: DndItem;
	destination: DndItem;
	pseudohidden = {};
	
	constructor (){
    }

	load(sourceItem: DndItem, destinationItem: DndItem) {
		this.source = sourceItem;
		this.destination = destinationItem;
    }

	swapItems() {
	    var collection = this.source.collection();
	    var sourceindex = collection.indexOf(this.source.item);
    	var destinationindex = collection.indexOf(this.destination.item);

        var tempSource = collection[sourceindex];
		collection[sourceindex] = collection[destinationindex];
		collection[destinationindex] = tempSource;
    }

	sameCollection() {
		console.log("-------------------------SAME")
		return this.source.collection().indexOf(this.destination.item) !== -1;
	}

	setSourceStatus(newstatus) {
	    this.source.item.status = newstatus
	}
    
    $emit(eventName) {
		this.source.scope.$emit(eventName, this);
	}

}