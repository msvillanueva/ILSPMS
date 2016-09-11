(function (app) {
    'use strict';

    app.directive('searchBar', searchBar);
    function searchBar() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'js/app/_layout/searchBar.html'
        }
    }

})(angular.module('ilsApp'));