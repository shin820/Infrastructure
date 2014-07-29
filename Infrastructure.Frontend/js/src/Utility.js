
define(function () {
    return {
        getUrlParams: function (url) {
            url = url || window.location.href;
            var re = /(?:\\?|#|&)([^&#?=]*)=([^&#?=]*)(?:$|&|#)/ig;
            var temp;
            var result = {};
            while ((temp = re.exec(url)) != null) {
                result[temp[1]] = temp[2];
            }
            return result;
        }
    };
})
