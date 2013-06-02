var applicationusersCtrl = ['$scope', 'cmsUsersApi', function ($scope, cmsUsersApi) {

	var loadUsers = function() {
		cmsUsersApi.getUsers(function(data) {
			$scope.data = data;
		});
	};

	loadUsers();

	$scope.save = function(user) {
		user.Edit = false;
		cmsUsersApi.putUser(user, function () {
		    user.Password = "";
		});
	};
	$scope.edit = function(user) {
		user.Edit = true;
	};
	$scope.delete = function() {
		console.log("delete");
	};
	$scope.cancelEdit = function (user) {
		user.Edit = false;
	};

	$scope.newItemAdd = function () {
		cmsUsersApi.postUser($scope.newitem, function () {
			loadUsers();
			reset();
		}, function (err) {
		    switch (err.status) {
		        case 409:
		            $scope.message = " uživatel '" +$scope.newitem.UserName+ "' uz existuje.";
		            break;
		        default:
		            $scope.message = "nesprávně vyplněné údaje.";
		    }
		});
	};

	var reset = function () {
		$scope.newitem.UserName = "";
		$scope.newitem.Password = "";
	    $scope.message = null;
	};

	$scope.$on("ngcClickEdit.showEdit", function () {
		$scope.newItemEdit = true;
	});
	
	$scope.$on("ngcClickEdit.showPreview", function () {
		reset();
		$scope.newItemEdit = false;
	});


}]