"use strict"
var GridElement = (function () {
	function GridElement(init) {
		var initData = init || {};
		this.Width = initData.width || 12;
		this.Id = this.generateId("");
		this.Content = initData.content || {};
		this.resources = initData.resources || {};
		this.Name = initData.name || "";
		this.Type = initData.type || "text";
	}

	GridElement.prototype.generateId = function (separator) {
		var delim = separator || "-";

		function S4() {
			return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
		}

		return (S4() + S4() + delim + S4() + delim + S4() + delim + S4() + delim + S4() + S4() + S4());
	};

	return GridElement;
})();
