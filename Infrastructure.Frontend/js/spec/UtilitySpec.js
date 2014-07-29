define(['src/Utility'], function (utility) {

    describe('test utility', function () {
        it('getUrlParams', function () {
            var result = utility.getUrlParams("http://asbsd/asd?a=1&b=2&c=3");
            expect(result.a).toBe('1');
            expect(result.b).toBe('2');
            expect(result.c).toBe('3');
        });
    });

});