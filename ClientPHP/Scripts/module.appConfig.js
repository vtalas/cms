angular.module('appConfigModule', [])
	.value("appConfig", {
		debug: true,
		baseUrl: "/ClientPhp/",
		applicationId: "c78ee05e-1115-480b-9ab7-a3ab3c0f6643"
	})
	.factory("chujFactory", function () {
		return "laskndalksd" + $(window).width();
	})
;
