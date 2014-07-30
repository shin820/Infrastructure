requirejs.config({
    baseUrl: 'js'
});

require(['config'], function () {

    requirejs.config({
        paths: {
            'jasmine': 'lib/jasmine-2.0.1/jasmine',
            'jasmine-html': 'lib/jasmine-2.0.1/jasmine-html',
            'jasmine-boot': 'lib/jasmine-2.0.1/boot',
            'blanket': 'lib/blanket-1.1.5.min',
            'jasmine-blanket': 'lib/jasmine-blanket-1.1.5'
        },
        shim: {
            'jasmine-boot': {
                deps: ['jasmine', 'jasmine-html'],
                exports: 'jasmine'
            },
            'jasmine-html': {
                deps: ['jasmine']
            },
            'jasmine-blanket': {
                deps: ['jasmine-boot', 'blanket'],
                exports: 'blanket'
            }
        }
    });

    require(['jquery', 'jasmine-boot', 'jasmine-blanket'], function ($, jasmine, blanket) {

        // blanket.options('debug', true);

        // include filter
        blanket.options('filter', 'js/');
        // exclude filter
        blanket.options('antifilter', ['lib', 'spec']);
        blanket.options('branchTracking', true);

        var jasmineEnv = jasmine.getEnv();
        jasmineEnv.addReporter(new jasmine.BlanketReporter());

        jasmineEnv.updateInterval = 1000;

        // add custom jasmine matchers
        beforeEach(function () {
            jasmine.addMatchers({
                toBeDate: function () {
                    return {
                        compare: function (actual, expected) {
                            return {
                                pass: actual.toString() === expected.toString()
                            };
                        }
                    };
                }
            });
        });

        var specs = [
            'spec/UtilitySpec'
        ];

        $(document).ready(function () {
            require(specs, function (spec) {
                window.onload();
            });
        });
    });
});