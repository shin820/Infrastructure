
define(function () {

    ///	<summary>
    ///     过滤字符串前后空格
    ///	</summary>
    ///	<param name="str" type="String">
    ///		表示要被过滤空格的字符串
    ///	</param>
    ///	<returns type="String" />
    function trim(str) {
        if (!str) {
            return "";
        }
        var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
        return str.replace(rtrim, "");
    }

    function getUrlParams(url) {
        url = url || window.location.href;
        var re = /(?:\\?|#|&)([^&#?=]*)=([^&#?=]*)(?:$|&|#)/ig;
        var temp;
        var result = {};
        while ((temp = re.exec(url)) != null) {
            result[temp[1]] = temp[2];
        }
        return result;
    }

    ///	<summary>
    ///     将日期对象格式化为指定格式的字符串形式.
    ///	</summary>
    ///	<param name="dtm" type="Date">
    ///		日期对象
    ///	</param>
    ///	<param name="fmt" type="String">
    ///		格式,值包括:yyyy表示4位年,MM表示2位月份,dd表示2位天,HH表示小时(24小时制),mm表示分钟,ss表示秒,S表示毫秒
    ///     例如: yyyy-MM-dd 会被格式化成 2014-02-18
    ///	</param>
    ///	<returns type="String" />
    function formateDate(dtm, fmt) {
        if (!dtm) {
            return null;
        }

        fmt = fmt || 'yyyy-MM-dd';
        var o = { "M+": dtm.getMonth() + 1, "d+": dtm.getDate(), "h+": dtm.getHours(), "H+": dtm.getHours(), "m+": dtm.getMinutes(), "s+": dtm.getSeconds(), "q+": Math.floor((dtm.getMonth() + 3) / 3), "S": dtm.getMilliseconds() };

        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (dtm.getFullYear() + "").substr(4 - RegExp.$1.length));
        }

        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return fmt;
    }

    ///	<summary>
    ///     将字符串按照指定格式,格式化为日期对象.
    ///	</summary>
    ///	<param name="str" type="String">
    ///		日期对象
    ///	</param>
    ///	<param name="fmt" type="String">
    ///		格式,值包括:yyyy表示4位年,MM表示2位月份,dd表示2位天,HH表示小时(24小时制),mm表示分钟,ss表示秒,S表示毫秒
    ///     例如: 2014-02-18 字符串可以使用 yyyy-MM-dd 格式进行格式化
    ///	</param>
    ///	<returns type="Date" />
    function parseDate(str, fmt) {
        if (!str) {
            return null;
        }

        fmt = fmt || 'yyyy-MM-dd';
        var year = 0, month = 0, date = 0, hours = 0, minutes = 0, seconds = 0, ms = 0;

        var o = ["y+", "M+", "d+", "h+", "H+", "m+", "s+", "S"];

        str = trim(str);
        fmt = trim(fmt);

        for (var k in o) {
            var regex = new RegExp("(" + o[k] + ")", "g");
            var arr;
            if ((arr = regex.exec(fmt)) != null) {
                var start = arr.index || 0;
                var end = regex.lastIndex || 0;

                if (start != -1 && end != -1) {
                    if (start > str.length || end > str.length)
                        continue;

                    var part = str.substring(start, end);
                    var val = parseInt(part);

                    if (!val)
                        continue;

                    switch (o[k]) {
                        case "y+":
                            year = val;
                            break;
                        case "M+":
                            month = val - 1;
                            break;
                        case "d+":
                            date = val;
                            break;
                        case "H+":
                        case "h+":
                            hours = val;
                            break;
                        case "m+":
                            minutes = val;
                            break;
                        case "s+":
                            seconds = val;
                            break;
                        case "S":
                            ms = val;
                            break;
                    }
                }
            }
        }

        return new Date(year, month, date, hours, minutes, seconds, ms);
    }

    ///	<summary>
    ///     日期加减计算
    ///	</summary>
    ///	<param name="dtm" type="Date">
    ///		日期对象
    ///	</param>
    ///	<param name="type" type="String">
    ///		类型,值包括 "day" 表示增加几天, "hour" 表示增加几小时, "minute" 表示增加几分钟, "second" 表示增加几秒
    ///	</param>
    ///	<param name="num" type="Number">
    ///		加减的数值,正数表示增加相应时间,负数表示减去相应的时间
    ///	</param>
    ///	<returns type="Date" />
    function addDate(dtm, type, num) {
        var time = dtm.getTime();
        switch (type) {
            case "day":
                time += num * 24 * 60 * 60 * 1000;
                break;
            case "hour":
                time += num * 60 * 60 * 1000;
                break;
            case "minute":
                time += num * 60 * 1000;
                break;
            case "second":
                time += num * 1000;
                break;
        }
        return new Date(time);
    }

    //判断是否为guid
    function isGuid(str) {
        var r = new RegExp("^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(,[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12})*$");
        return r.test(str.toString());
    }

    return {

        trim: trim,

        getUrlParams: getUrlParams,

        formateDate: formateDate,

        parseDate: parseDate,

        addDate: addDate,

        isGuid: isGuid
    };
})
