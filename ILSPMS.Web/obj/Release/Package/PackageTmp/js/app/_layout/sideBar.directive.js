(function (app) {
    'use strict';

    app.directive('sideBar', sideBar);
    function sideBar() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'js/app/_layout/sideBar.html'
        }
    }

})(angular.module('ilsApp'));