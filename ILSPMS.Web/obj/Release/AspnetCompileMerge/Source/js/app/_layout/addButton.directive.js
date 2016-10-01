(function (app) {
    'use strict';

    app.directive('addButton', addButton);
    function addButton() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'js/app/_layout/addButton.html',
            scope: {
                label: '@',
                create: '&'
            }
        }
    }

})(angular.module('ilsApp'));