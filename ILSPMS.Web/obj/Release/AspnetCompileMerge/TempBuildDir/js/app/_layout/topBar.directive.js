(function (app) {
    'use strict';

    app.directive('topBar', topBar);
    function topBar() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/js/app/_layout/topBar.html'
        }
    }

})(angular.module('ilsApp'));