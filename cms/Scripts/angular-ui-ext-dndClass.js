var StatusEnum;
(function (StatusEnum) {
    StatusEnum._map = [];
    StatusEnum.DRAGGED = 1;
    StatusEnum.DRAGEND = 5;
    StatusEnum.DRAGOLD = 9;
    StatusEnum.DROPPED = 3;
    StatusEnum.PSEUDOHIDDEN = 333;
    StatusEnum.SWAPPED = 2;
})(StatusEnum || (StatusEnum = {}));

var DndItem = (function () {
    function DndItem(item, scope, element, namespace) {
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
    DndItem.prototype.hasCollection = function () {
        return (this.scope && this.scope.$parent && this.scope.$parent.$collection);
    };
    DndItem.prototype.isClone = function () {
        return this.item.isClone;
    };
    DndItem.prototype.removeFromCollection = function () {
        var index;
        if(!this.item || !this.hasCollection()) {
            return -1;
        }
        index = this.findById(this.item);
        this.collection().splice(index, 1);
    };
    DndItem.prototype.findById = function (item) {
        return this.findBy("$$hashKey", item.$$hashKey);
    };
    DndItem.prototype.findBy = function (key, value) {
        var collection = this.collection();
        var len = collection.length;
        for(var i = 0; i < len; i++) {
            if(collection[i][key] === value) {
                return i;
            }
        }
        return -1;
    };
    return DndItem;
})();
var DragAndDropObjts = (function () {
    function DragAndDropObjts() { }
    DragAndDropObjts.prototype.load = function (sourceItem, destinationItem) {
        this.source = sourceItem;
        this.destination = destinationItem;
        this.pseudohidden = new DndItem({
        }, {
        }, {
        }, "");
    };
    DragAndDropObjts.prototype.swapItems = function () {
        var collection = this.source.collection();
        var sourceindex = this.source.findById(this.source.item);
        var destinationindex = this.source.findById(this.destination.item);

        var tempSource = collection[sourceindex];
        collection[sourceindex] = collection[destinationindex];
        collection[destinationindex] = tempSource;
    };
    DragAndDropObjts.prototype.changeParent = function () {
        this.removeOrHideInSource();
        this.showOrPush();
        this.source.scope = this.destination.scope;
        this.source.item.ParentId = this.destination.item.ParentId;
    };
    DragAndDropObjts.prototype.hide = function (itemToHide) {
        itemToHide.statusclass = "pseudohidden";
        itemToHide.status = StatusEnum.PSEUDOHIDDEN;
    };
    DragAndDropObjts.prototype.show = function (itemToHide, newstatus) {
        itemToHide.statusclass = "";
        itemToHide.status = newstatus;
    };
    DragAndDropObjts.prototype.removeOrHideInSource = function () {
        if(this.source.isClone()) {
            this.source.removeFromCollection();
        } else {
            this.hide(this.source.item);
            this.pseudohidden.item = this.source.item;
            this.pseudohidden.scope = this.source.scope;
        }
    };
    DragAndDropObjts.prototype.showOrPush = function () {
        var collectiondest = this.destination.collection();
        var destinationindex = this.destination.findById(this.destination.item);
        var clonedindexdest = this.destination.findBy("status", StatusEnum.PSEUDOHIDDEN);
        if(clonedindexdest !== -1) {
            this.show(collectiondest[clonedindexdest], StatusEnum.DRAGGED);
            this.pseudohidden.item = null;
            this.source.item = collectiondest[clonedindexdest];
        } else {
            var clone = angular.extend({
            }, this.source.item);
            clone.status = StatusEnum.DRAGGED;
            clone.statusclass = "";
            clone.isClone = true;
            this.source.item = clone;
            this.pushToIndex(clone, collectiondest, destinationindex);
        }
    };
    DragAndDropObjts.prototype.pushToIndex = function (item, collection, index) {
        collection.splice(index, 0, item);
    };
    DragAndDropObjts.prototype.sameCollection = function () {
        return this.source.findById(this.destination.item) !== -1;
    };
    DragAndDropObjts.prototype.setSourceStatus = function (newstatus) {
        this.source.item.status = newstatus;
    };
    DragAndDropObjts.prototype.dragEnd = function () {
        var position = this.destination.findById(this.destination.item);
        this.setSourceStatus(StatusEnum.DRAGEND);
        this.source.item.isClone = false;
        this.source.item.Position = position;
        this.pseudohidden.removeFromCollection();
    };
    DragAndDropObjts.prototype.$emit = function (eventName) {
        this.source.scope.$emit(eventName, this);
    };
    return DragAndDropObjts;
})();
