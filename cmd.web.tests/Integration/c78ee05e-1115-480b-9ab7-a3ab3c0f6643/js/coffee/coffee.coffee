# Type here! 
Widget =
  hide: ->
    @element.animate
      opacity: 0.0
      top: -10


  show: ->
    @element.animate
      opacity: 1.0
      top: 0


  element: $(".widget")