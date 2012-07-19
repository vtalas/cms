###
@reference jquery-1.7.2.js
@reference angular.js
###

module = angular.module("templateExt",[ ])

module.config ($provide )->
	$provide.factory "gridtemplate", ($templateCache) ->
		(type)->
			elementType = type ? type : "text";
			if (!$templateCache.get(elementType))
				$.ajax(
					url: '/Templates/GridElementTmpl'
					data: { type: elementType }
					async: false
					success: (data) ->
						$templateCache.put(elementType, data);
				)
			$templateCache.get(elementType)
	1

