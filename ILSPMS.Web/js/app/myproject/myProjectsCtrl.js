﻿(function (app) {
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
        $scope.popMenu = popMenu;
        $scope.hideMenu = hideMenu;

        function clearSearch() {
            $scope.filter = '';
            search();
        }

        function search() {
            $scope.loadingData = true;

            var config = {
                params: {
                    filter: $scope.filter,
                    all: $scope.all,
                    divisionID: null
                }
            };

            apiService.get('/api/projects/', config,
                loadComplete,
                notificationService.responseFailed);
        }

        function loadComplete(result) {
            $scope.loadingData = false;
            if ($scope.filter && $scope.filter.length) {
                notificationService.displayInfo(result.data.items.length + (result.data.items.length > 1 ? ' records found' : ' record found'));
            }

            $scope.projects = result.data.items;
        }

        function popMenu($event, row) {
            row.Hovered = true;
        }

        function hideMenu($event, row) {
            row.Hovered = false;
        }

        $scope.search();
    }

})(angular.module('ilsApp'));