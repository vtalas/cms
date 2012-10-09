var DndItem = (function () {
    function DndItem(item, scope, element, namespace) {
        this.item = {
        };
        this.scope = {
        };
        this.element = {
        };
        this.item = item;
        this.scope = scope , this.element = element , this.namespace = namespace;
    }
    DndItem.prototype.collection = function () {
        return this.scope.$parent.$collection;
    };
    return DndItem;
})();
var DragAndDropObjts = (function () {
    function DragAndDropObjts() {
        this.pseudohidden = {
        };
    }
    DragAndDropObjts.prototype.load = function (sourceItem, destinationItem) {
        this.source = sourceItem;
        this.destination = destinationItem;
    };
    DragAndDropObjts.prototype.sameCollection = function () {
        console.log("-------------------------SAME");
        return this.source.collection().indexOf(this.destination.item) !== -1;
    };
    DragAndDropObjts.prototype.setSourceStatus = function (newstatus) {
        this.source.item.status = newstatus;
    };
    DragAndDropObjts.prototype.$emit = function (eventName) {
        this.source.scope.$emit(eventName, this);
    };
    return DragAndDropObjts;
})();
