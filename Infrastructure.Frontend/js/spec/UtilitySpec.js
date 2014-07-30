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
            expect(utility.parseDate('2000-12-31 21:05:22', 'yyyy-MM-dd HH:mm:ss')).toBeDate(new Date(2000, 11, 31, 21, 5, 22));
        });

        it('addDate', function () {
            var date = new Date(2000, 0, 1, 1, 1, 1);
            expect(utility.addDate(date, 'day', 12)).toBeDate(new Date(2000, 0, 13, 1, 1, 1));
            expect(utility.addDate(date, 'day', -3)).toBeDate(new Date(1999, 11, 29, 1, 1, 1));
            expect(utility.addDate(date, 'hour', 5)).toBeDate(new Date(2000, 0, 1, 6, 1, 1));
            expect(utility.addDate(date, 'hour', -1)).toBeDate(new Date(2000, 0, 1, 0, 1, 1));
            expect(utility.addDate(date, 'minute', 30)).toBeDate(new Date(2000, 0, 1, 1, 31, 1));
            expect(utility.addDate(date, 'minute', -10)).toBeDate(new Date(2000, 0, 1, 0, 51, 1));
            expect(utility.addDate(date, 'second', 50)).toBeDate(new Date(2000, 0, 1, 1, 1, 51));
            expect(utility.addDate(date, 'second', -20)).toBeDate(new Date(2000, 0, 1, 1, 0, 41));
        });

        it('isGuid', function () {
            expect(utility.isGuid('abdsas-asdfad')).toBeFalsy();
            expect(utility.isGuid('345C472A-0D84-41EC-B847-B61F6371A6B0')).toBeTruthy();
        });
    });
});