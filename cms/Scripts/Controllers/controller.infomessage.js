var infomessages = ['$scope', function ($scope, $routeProvider) {

    $scope.$on("set-message", function (e, data) {
        $scope.message = data.message;
        $scope.type = data.type;
    });
    $scope.$on("reset-message", function (e, data) {
        $scope.message = null;
    });
    
    $scope.$on("$routeChangeStart", function(parameters) {
        $scope.message = null;
    });

    $scope.messagetype = function () {
        switch ($scope.type) {
            case "error":
                return "alert-error";
            case "success":
                return "alert-success";
            case "danger":
                return "alert-danger";
            default:
                return "alert-error";

        }
    };
}];