(function (app) {
    'use strict';

    app.controller('myProjectsCtrl', myProjectsCtrl);
    myProjectsCtrl.$inject = ['$scope', '$uibModal', 'apiService', 'notificationService', 'cambria'];

    function myProjectsCtrl($scope, $uibModal, apiService, notificationService, cambria) {
        $scope.pageClass = 'page-myprojects';
        $scope.loadingData = true;
        $scope.projects = [];
        $scope.tableRowCollection = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        function clearSearch() {
            $scope.filter = '';
            search();
        }

        function search() {
            $scope.loadingData = true;

            apiService.get('/api/myprojects/', null,
                loadComplete,
                notificationService.responseFailed);
        }

        function loadComplete(result) {
            $scope.loadingData = false;
            if ($scope.filter && $scope.filter.length) {
                notificationService.displayInfo(result.data.items.length + (result.data.items.length > 1 ? ' records found' : ' record found'));
            }

            $scope.tableRowCollection = result.data.items;
            $scope.divisions = [].concat($scope.tableRowCollection);
        }

        $scope.search();
    }

})(angular.module('ilsApp'));