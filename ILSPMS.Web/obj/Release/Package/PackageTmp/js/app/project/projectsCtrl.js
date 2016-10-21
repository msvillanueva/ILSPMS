(function (app) {
    'use strict';

    app.controller('projectsCtrl', projectsCtrl);
    projectsCtrl.$inject = ['$scope', '$uibModal', 'apiService', 'notificationService', 'cambria'];

    function projectsCtrl($scope, $uibModal, apiService, notificationService, cambria) {
        $scope.pageClass = 'page-projects';
        $scope.loadingData = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.projects = [];
        $scope.tableRowCollection = [];
        $scope.pms = [];
        $scope.divisions = [];
        $scope.divisionID = '0';
        $scope.years = [];
        $scope.selectedYear = $scope.year.toString();

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.archive = archive;
        $scope.create = create;

        function clearSearch() {
            $scope.filter = '';
            search();
        }

        function search() {
            $scope.loadingData = true;
            var config = {
                params: {
                    filter: $scope.filter,
                    divisionID: $scope.divisionID,
                    year: $scope.selectedYear
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

            $scope.tableRowCollection = result.data.items;
            $scope.projects = [].concat($scope.tableRowCollection);
        }

        function archive(idx, row) {
            cambria.cConfirm('Archive this project?', 'CONFIRM ACTION', function (click) {
                if (click) {
                    $scope.projects.splice(idx, 1);
                    apiService.post(
                        '/api/projects/remove/',
                        row,
                        function (response) {
                            if (response.data.success)
                                notificationService.displaySuccess(row.Name + ' was removed from the list.');
                            else
                                notificationService.displayError(response.data.message);
                        },
                        notificationService.responseFailed
                    );
                }
            });
        }

        function create(row) {

            if (row)
                $scope.viewModel = angular.copy(row);
            else
                $scope.viewModel = { ID: 0 };

            $uibModal.open({
                templateUrl: 'js/app/project/projectViewModal.html',
                controller: 'projectViewModalCtrl',
                scope: $scope,
                backdrop: 'static',
                keyboard: false
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function init() {
            apiService.get('/api/divisions/', null,
                function (result) {
                    $scope.divisions = result.data.items;
                },
                notificationService.responseFailed);

            apiService.get('/api/users/pms', null,
                function (result) {
                    $scope.pms = result.data.items;
                },
                notificationService.responseFailed);

            apiService.post('/api/projects/years', null,
                function (result) {
                    $scope.years = result.data.items;

                },
                notificationService.responseFailed);

            $scope.search();
        }

        init();
    }

})(angular.module('ilsApp'));