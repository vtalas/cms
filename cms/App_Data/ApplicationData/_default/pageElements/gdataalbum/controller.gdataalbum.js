
/*
@reference ../showdown.js
@reference ../jquery-1.7.2.js
@reference ../angular.js
*/


(function() {
  var gdataalbum;

  gdataalbum = function($scope, gdataPhotos) {
    var getCurrentAlbum, gridelement;
    gdataalbum.$inject = ["$scope", "gdataPhotos"];
    getCurrentAlbum = function(item) {
      debugger;
      var currentAlbum, data;
      if (item.Content !== null) {
        data = JSON.parse(item.Content);
        if ($scope.albums) {
          currentAlbum = $scope.albums[data.gdataAlbumId];
        }
      }
      return currentAlbum;
    };
    gridelement = $scope.$parent.gridelement;
    $scope.showGdataAlbumsValue = false;
    gdataPhotos.getAlbums(function(data) {
      $scope.albums = data;
      $scope.currentAlbum = getCurrentAlbum(gridelement);
      return 1;
    });
    $scope.$on("ngcClickEdit-showPreview", function(e, data) {
      return $scope.$emit("gridelement-save", gridelement);
    });
    $scope.addAlbum = function(id) {
      gridelement.Content = JSON.stringify({
        gdataAlbumId: id
      });
      $scope.currentAlbum = getCurrentAlbum(gridelement);
      $scope.showGdataAlbumsValue = $scope.hideGdataAlbums();
      $scope.$emit("gridelement-save", gridelement);
      return $scope.$apply;
    };
    $scope.hasAlbum = function() {
      return gridelement.Content !== null;
    };
    $scope.isEditMode = function() {
      return !!gridelement.Edit;
    };
    $scope.showGdataAlbums = function() {
      return $scope.showGdataAlbumsValue = true;
    };
    $scope.hideGdataAlbums = function() {
      return $scope.showGdataAlbumsValue = false;
    };
    $scope.isShowGdataAlbums = function() {
      return !!gridelement.Edit && $scope.showGdataAlbumsValue;
    };
    return 1;
  };

  window.gdataalbum = gdataalbum;

}).call(this);
