class DndItem {
    item = {};
    scope = {};
    element = {};
    namespace: string;
    constructor (item, scope, element, namespace) {
    
    }
    collection() {
        return this.scope.$parent.$collection;
    }

}

class DragAndDropObj {
    source: DndItem;
    destination: DndItem;
	pseudohidden = {};
	
	constructor (){
	}

	setSourceStatus(newstatus) {
	    this.source.item.status = newstatus
	};
    
    $emit(eventName) {
		//this.source.scope.$emit(eventName, this);
	};

};