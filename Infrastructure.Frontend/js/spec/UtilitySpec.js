define(['src/Utility'], function (utility) {

    describe('Utility模块', function () {

        it('getUrlParams', function () {
            var result = utility.getUrlParams("http://asbsd/asd?a=1&b=2&c=3");
            expect(result.a).toBe('1');
            expect(result.b).toBe('2');
            expect(result.c).toBe('3');
        });

        it('formateDate', function () {
            var d = new Date(2000, 11, 31);
            expect(utility.formateDate(d)).toBe('2000-12-31');
            expect(utility.formateDate(d, 'yyyy')).toBe('2000');
            expect(utility.formateDate(d, 'MM')).toBe('12');
            expect(utility.formateDate(d, 'dd')).toBe('31');
            expect(utility.formateDate(d, 'yyyy/dd/MM')).toBe('2000/31/12');
        });

        it('parseDate', function () {
            expect(utility.parseDate('2000-12-31 21:05:22', 'yyyy-MM-dd HH:mm:ss').toString()).toBe(new Date(2000, 11, 31, 21, 5, 22).toString());
        });
    });
});