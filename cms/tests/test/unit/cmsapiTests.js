/* jasmine specs for controllers go here */
describe('cms Api module', function () {

	describe('Gridapi tests', function () {
        var scope, ctrl, $httpBackend;

        beforeEach(angular.mock.module('cmsapi'));

        beforeEach(inject(function(_$httpBackend_, $rootScope, $controller) {

            $httpBackend = _$httpBackend_;
            $httpBackend.expectGET('/adminApi/1111/grids').
                respond([{name: 'Nexus S'}, {name: 'Motorola DROID'}]);

            // scope = $rootScope.$new();
            //ctrl = $controller(PhoneListCtrl, {$scope: scope});

        }));

        it('should load GridApi', inject(function(_$httpBackend_, GridApi) {
            var rs = GridApi.grids({
                    applicationId : "1111"
                }, function(data){
                    console.log(data, "xx");
                }
            );

            console.log(rs);
        }));

    });
});
