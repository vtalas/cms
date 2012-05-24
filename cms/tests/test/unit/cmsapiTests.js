/* jasmine specs for controllers go here */
describe('cms Api module', function () {

	describe('Gridapi tests', function () {
        var scope, ctrl, $httpBackend;

        beforeEach(angular.mock.module('ngResource'));
        beforeEach(angular.mock.module('cmsapi'));
        beforeEach(inject(function(_$httpBackend_, $rootScope, $controller) {

            $httpBackend = _$httpBackend_;
            $httpBackend.expectPOST('/adminApi/test1/grids').
                respond([{name: 'Nexus S'}, {name: 'Motorola DROID'}]);

            // scope = $rootScope.$new();
            //ctrl = $controller(PhoneListCtrl, {$scope: scope});

         }));

        it('should load GridApi', inject(function(version,GridApi) {
            expect(version).toEqual('gridapi1.0.1');
            expect(GridApi).toBeDefined();
        }));
        it('should load GridApi', inject(function(_$httpBackend_, GridApi) {
            console.log(GridApi.get());
        }));

    });
});
