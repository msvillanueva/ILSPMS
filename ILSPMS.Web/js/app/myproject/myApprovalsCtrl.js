(function (app) {
    'use strict';

    app.controller('myApprovalsCtrl', myApprovalsCtrl);
    myApprovalsCtrl.$inject = ['$scope', '$uibModal', 'apiService', 'notificationService', 'cambria'];

    function myApprovalsCtrl($scope, $uibModal, apiService, notificationService, cambria) {
        $scope.pageClass = 'page-myprojects';
        $scope.loadingData = true;
        $scope.projects = [];
        $scope.tableRowCollection = [];
        $scope.topMilestone = 0;
        $scope.forApproval = true;

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
                    forApproval: $scope.forApproval
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

        $scope.search();
    }

})(angular.module('ilsApp'));