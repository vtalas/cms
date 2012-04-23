/* jasmine-like end2end tests go here */

describe('PhoneCat App', function () {

	describe('Phone list view', function () {

		beforeEach(function () {
			browser().navigateTo('../../app/index.html');
		});


		it('should filter the phone list as user types into the search box', function () {
			expect(repeater('.phones li').count()).toBe(3);
			pause();
			input('query').enter('nexus');
			expect(repeater('.phones li').count()).toBe(1);

			input('query').enter('motorola');
			expect(repeater('.phones li').count()).toBe(2);
		});
	});

	describe('PhoneListCtrl', function () {
		it('should create "phones" model with 3 phones', function () {
			var ctrl = new PhoneListCtrl();
			expect(ctrl.phones.length).toBe(3);
		});
	});

});
