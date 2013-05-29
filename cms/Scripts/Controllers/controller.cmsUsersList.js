var cmsUsersList = ['$scope', 'cmsUsersApi', function ($scope, cmsUsersApi) {

	var loadUsers = function() {
		cmsUsersApi.getUsers(function(data) {
			$scope.data = data;
		});
	};

	loadUsers();

	$scope.save = function(user) {
		user.Edit = false;
		console.log("save");
	};
	$scope.edit = function(user) {
		user.Edit = true;
		console.log("edit");
	};
	$scope.delete = function(parameters) {
		console.log("delete");
	};
	$scope.cancelEdit = function (user) {
		user.Edit = false;
		console.log("cancel");
	};


	$scope.newItemAdd = function () {
		console.log($scope.newitem);
		cmsUsersApi.postUser($scope.newitem, function (data) {
			loadUsers();
			newItemReset();
		}, function (err) {
			console.log(err);
		});
	};

	var newItemReset = function () {
		$scope.newitem.UserName = "";
		$scope.newitem.Password = "";
	};

	$scope.$on("ngcClickEdit.showEdit", function () {
		$scope.newItemEdit = true;
	});
	
	$scope.$on("ngcClickEdit.showPreview", function () {
		newItemReset();
		$scope.newItemEdit = false;
	});


}]