/* jasmine specs for controllers go here */
describe('cms Api module', function () {

	describe('Gridapi tests', function () {
        var scope, ctrl, $httpBackend;

        beforeEach(angular.mock.module('cmsapi'));

        beforeEach(inject(function(_$httpBackend_, $rootScope, $controller) {

            $httpBackend = _$httpBackend_;
            $httpBackend.expectGET('/adminApi/test1/grids').
                respond([{name: 'Nexus S'}, {name: 'Motorola DROID'}]);

            // scope = $rootScope.$new();
            //ctrl = $controller(PhoneListCtrl, {$scope: scope});

        }));

        it('should load GridApi', inject(function(module) {
            expect(module).toEqual('gridapi');
        }));

        it('should load GridApi', inject(function(_$httpBackend_, GridApi) {
            var rs = GridApi.get(null, function(data){
                    console.log(data, "xx");
                }
            );

            console.log(rs);
        }));

    });
});
