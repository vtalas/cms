/* jasmine specs for controllers go here */
describe('PhoneCat controllers', function () {

	describe('aaaPhoneListCtrl', function () {

		it('should create "phones" model with 3 phones', function () {
			var scope, $browser, ctrl;

			beforeEach(function() {
				scope = angular.scope();
				$browser = scope.$service('$browser');

				$browser.xhr.expectGET('api/DataServiceGradients/GetAll')
					.respond([{ name: 'Nexus S' },
							{ name: 'Motorola DROID' }]);
							$browser.xhr('api/DataServiceGradients', function (response, e) {
									console.log(response);
								});

//								ctrl = scope.$new(PhoneListCtrl);
			});

		});
	});


});
