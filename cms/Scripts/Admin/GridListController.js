/**
 * Created with JetBrains WebStorm.
 * User: ladik
 * Date: 14.5.12
 * Time: 21:43
 * To change this template use File | Settings | File Templates.
 */
function GridListController($scope, $http, $routeParams, appSettings, GridApi) {
    var self = this;
    $scope.data = GridApi.get(null, function(d){
        console.log(d);
    });


    $scope.add = function () {
        var newitem = {Name : $scope.newName};
        $http({ method: 'POST', url: '/adminApi/' + appSettings.Name + '/AddGrid', data: {data : newitem} })
            .success(function (data, status, headers, config) {
                $scope.data.push(data);
            })
            .error(function (data, status, headers, config) {

            });

        $scope.newName = '';
    };
}