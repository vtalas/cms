###
@reference jquery-1.7.2.js
@reference angular.js
###

module = angular.module("templateExt",[ ])

module.config ($provide )->
	$provide.factory "gridtemplate", ($templateCache) ->
		(type)->
			elementType = type ? type : "text";
			$templateCache.get(elementType)

	$provide.factory "menuItemTemplate", ($templateCache) ->
		(type)->
			elementType = type ? type : "text";
			$templateCache.get(elementType)
			
	$provide.factory "gridtemplateClient", ($templateCache) ->
		(type)->
			elementType = type ? type : "text"
			$.ajax(
				url: '/Templates/GridElementTmplClient'
				data: { type: elementType }
				async: false
			).responseText
	1



