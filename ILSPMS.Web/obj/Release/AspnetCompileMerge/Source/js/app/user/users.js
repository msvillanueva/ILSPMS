(function (app) {
    'use strict';

    app.controller('usersCtrl', usersCtrl);
    usersCtrl.$inject = ['$scope', '$uibModal', 'apiService', 'notificationService', 'cambria'];

    function usersCtrl($scope, $uibModal, apiService, notificationService, cambria) {
        $scope.pageClass = 'page-users';
        $scope.loadingData = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.users = [];
        $scope.tableRowCollection = [];
        $scope.roles = [];
        $scope.divisions = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.archive = archive;
        $scope.toggleLock = toggleLock;
        $scope.create = create;

        function clearSearch() {
            $scope.filter = '';
            search();
        }

        function search() {
            $scope.loadingData = true;
            var config = {
                params: {
                    filter: $scope.filter
                }
            };

            apiService.get('/api/users/', config,
                loadComplete,
                notificationService.responseFailed);
        }

        function loadComplete(result) {
            $scope.loadingData = false;
            if ($scope.filter && $scope.filter.length) {
                notificationService.displayInfo(result.data.items.length + (result.data.items.length > 1 ? ' records found' : ' record found'));
            }

            $scope.tableRowCollection = result.data.items;
            $scope.users = [].concat($scope.tableRowCollection);
        }

        function archive(idx, row) {
            cambria.cConfirm('Archive this user account?', 'CONFIRM ACTION', function (click) {
                if (click) {
                    $scope.users.splice(idx, 1);
                    apiService.post(
                        '/api/users/remove/',
                        row,
                        function (response) {
                            if (response.data.success)
                                notificationService.displaySuccess(row.FullName + ' was removed from the list.');
                            else
                                notificationService.displayError(response.data.message);
                        },
                        notificationService.responseFailed
                    );
                }
            });
        }

        function toggleLock(row, lock) {
            row.IsLocked = lock;
            apiService.post(
                '/api/users/lockunlock/',
                row,
                function (response) {
                    if (response.data.success)
                        notificationService.displaySuccess(row.FullName + '\'s account is now ' + (lock ? 'locked.' : 'unlocked.'));
                    else
                        notificationService.displayError(response.data.message);
                },
                notificationService.responseFailed
            );
        }

        function create(row) {
            
            if (row)
                $scope.viewModel = angular.copy(row);
            else
                $scope.viewModel = { ID: 0, RoleID: $scope.roles[1].ID };

            $uibModal.open({
                templateUrl: 'js/app/user/userViewModal.html',
                controller: 'userViewModalCtrl',
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

            apiService.get('/api/roles/', null,
                function (result) {
                    $scope.roles = result.data.items;
                },
                notificationService.responseFailed);

            $scope.search();
        }

        init();
    }

})(angular.module('ilsApp'));