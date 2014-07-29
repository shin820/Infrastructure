define([], function () {
    requirejs.config({
        baseUrl: 'js',
        paths: {
            jquery: 'lib/jquery-1.11.1',
            underscore: 'lib/underscore-1.6.0.min',
            backbone: 'lib/backbone-1.1.0.min'
            //templates: '../templates'
        },
        shim: {
            backbone: {
                deps: ['underscore', 'jquery'],
                exports: 'Backbone'
            },
            underscore: {
                exports: '_'
            }
        }
    });
});