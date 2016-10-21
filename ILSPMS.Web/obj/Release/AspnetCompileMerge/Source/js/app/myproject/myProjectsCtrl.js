(function (app) {
    'use strict';

    app.controller('myProjectsCtrl', myProjectsCtrl);
    myProjectsCtrl.$inject = ['$scope', '$uibModal', 'apiService', 'notificationService', 'cambria'];

    function myProjectsCtrl($scope, $uibModal, apiService, notificationService, cambria) {
        $scope.pageClass = 'page-myprojects';
        $scope.loadingData = true;
        $scope.projects = [];
        $scope.tableRowCollection = [];
        $scope.topMilestone = 0;
        $scope.forApproval = false;
        $scope.divisions = [];
        $scope.divisionID = '0';
        $scope.years = [];
        $scope.selectedYear = $scope.year.toString();
        $scope.viewby = 6;
        $scope.currentPage = 1;
        $scope.itemsPerPage = $scope.viewby;
        $scope.maxSize = 5;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.popMenu = popMenu;
        $scope.hideMenu = hideMenu;
        $scope.submit = submit;
        $scope.approve = approve;

        function clearSearch() {
            $scope.filter = '';
            search();
        }

        function search() {
            $scope.loadingData = true;
            var config = {
                params: {
                    filter: $scope.filter,
                    year: $scope.selectedYear,
                    forApproval: $scope.forApproval,
                    divisionID: $scope.divisionID
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
            $scope.topMilestone = result.data.ms;
            $scope.projects = result.data.items;
        }

        function submit(row) {
            cambria.cConfirm('Are you sure you want to commit project<br/><b>' + row.Name + '</b>?', 'CONFIRM ACTION', function (click) {
                if (click) {
                    apiService.post(
                        '/api/projects/submit/',
                        row,
                        function (response) {
                            if (response.data.success)
                                $scope.search();
                            else
                                notificationService.displayError(response.data.message);
                        },
                        notificationService.responseFailed
                    );
                }
            });
        }

        function approve(row) {
            cambria.cConfirm('Are you sure you want to approve project<br/><b>' + row.Name + '</b>?', 'CONFIRM ACTION', function (click) {
                if (click) {
                    apiService.post(
                        '/api/projects/approve/',
                        row,
                        function (response) {
                            if (response.data.success)
                                $scope.search();
                            else
                                notificationService.displayError(response.data.message);
                        },
                        notificationService.responseFailed
                    );
                }
            });
        }

        function popMenu($event, row) {
            row.Hovered = true;
        }

        function hideMenu($event, row) {
            row.Hovered = false;
        }

        function init() {
            if ($scope.isDirector) {
                apiService.get('/api/divisions/', null,
                    function (result) {
                        $scope.divisions = result.data.items;
                    },
                    notificationService.responseFailed);
            }

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