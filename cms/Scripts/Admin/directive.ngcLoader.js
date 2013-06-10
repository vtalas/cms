var ngcLoader = function () {
	return {
		template: '<img src="/Content/loader16.gif" alt="loader" />',
		replace: true,
		scope: {
			ngcLoader: "="
		},
		link: function (scope) {

		}
	};
};