var DndItem = (function () {
    function DndItem(item, scope, element, namespace) {
        this.item = {
        };
        this.scope = {
        };
        this.element = {
        };
    }
    DndItem.prototype.collection = function () {
        return this.scope.$parent.$collection;
    };
    return DndItem;
})();
var DragAndDropObj = (function () {
    function DragAndDropObj() {
        this.pseudohidden = {
        };
    }
    DragAndDropObj.prototype.setSourceStatus = function (newstatus) {
        this.source.item.status = newstatus;
    };
    DragAndDropObj.prototype.$emit = function (eventName) {
    };
    return DragAndDropObj;
})();
; ;
