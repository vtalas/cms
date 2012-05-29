/**/
///<reference path="../showdown.js"/>
///<reference path="../jquery-1.7.2.js"/>
///<reference path="../angular.js"/>

var novinka = function($scope, $http,appSettings) {
    $scope.data = $scope.$parent.item;


    $scope.data.Content = angular.fromJson($scope.data.Content) || {header:"",text:""};

    //console.log("nka", $scope.data.Content, appSettings);
    //$scope.data.Content.header = $scope.$parent.item.Content.header || "AA";
    //$scope.data.Content.text = $scope.$parent.item.Content.text || "XXX";

    var converter = new Showdown.converter();
    var self = this;
    var toHtml = function (markdown) {
        var a = true;

        return converter.makeHtml(markdown);
    };

    $scope.headerHtml = function () {
        return toHtml($scope.data.Content.header);
    };

    $scope.textHtml = function () {
        return toHtml($scope.data.Content.text);
    };

    $scope.thumb = function () {
        return $scope.data.thumb;
    };


    ////////////////////////////
    $scope.add = function() {

    };

}
novinka.$inject = ['$scope', '$http', 'appSettings'];

