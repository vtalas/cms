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
	$scope.remove = function() {
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
		    var message;
		    switch (err.status) {
		        case 409:
		            message = " uživatel '" +$scope.newitem.UserName+ "' uz existuje.";
		            break;
		        default:
		            message = "nesprávně vyplněné údaje.";
		    }
		    $scope.$emit("set-message", {message:message});
		});
	};

	var reset = function () {
		$scope.newitem.UserName = "";
		$scope.newitem.Password = "";
		$scope.$emit("reset-message");
	};

	$scope.$on("ngcClickEdit.showEdit", function () {
		$scope.newItemEdit = true;
	});
	
	$scope.$on("ngcClickEdit.showPreview", function () {
		reset();
		$scope.newItemEdit = false;
	});


}]