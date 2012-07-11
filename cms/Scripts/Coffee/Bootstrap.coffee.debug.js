(function() {
  /*
  @reference ../jquery-1.7.2.js
  @reference ../angular.js
  */
  var bootstrap;
  bootstrap = function($scope, $http, $element, colorsonly, $filter) {
    $scope.data = $.parseJSON($element.data("model"));
    $scope.colorsrefonly = colorsonly;
    $scope.showColorPicker = function($event, item) {
      var all, basiccolors, el;
      all = $scope.data;
      basiccolors = $filter("nameType")(all, "basiccolor", item.value);
      el = $event.currentTarget;
      console.log(basiccolors, $(el).val());
      $(el).typeahead({
        source: basiccolors,
        updater: function(val) {
          $(el).val(val);
          return $scope.data[name].value = val;
        },
        items: 11
      });
      1;
      colorPicker($event);
      return colorPicker.exportColor = function() {
        return item.value = "#" + colorPicker.CP.hex;
      };
    };
    $scope.refresh = function() {
      return $http({
        method: "POST",
        url: "/bootstrap/Refresh",
        data: $scope.data
      }).success(function(data, status, headers, config) {
        console.log(data);
        return $scope.refreshtoken = new Date;
      });
    };
    $scope.toggleValue = function(item) {
      if ($scope.hider[item]) {
        return true;
      } else {
        return false;
      }
    };
    $scope.toggle = function(item) {
      $scope.hider[item] = !$scope.toggleValue(item);
      return console.log($scope.hider, item);
    };
    return 1;
  };
  window.bootstrap = bootstrap;
}).call(this);
