(function (app) {
    'use strict';

    app.controller('divisionsCtrl', divisionsCtrl);
    divisionsCtrl.$inject = ['$scope', '$uibModal', 'apiService', 'notificationService', 'cambria'];

    function divisionsCtrl($scope, $uibModal, apiService, notificationService, cambria) {
        $scope.pageClass = 'page-divisions';
        $scope.loadingData = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.divisions = [];
        $scope.tableRowCollection = [];

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

            apiService.get('/api/divisions/', null,
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

        function archive(idx, row) {
            cambria.cConfirm('Archive this division?', 'CONFIRM ACTION', function (click) {
                if (click) {
                    $scope.divisions.splice(idx, 1);
                    apiService.post(
                        '/api/divisions/remove/',
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

        function create(model) {
            if (model)
                $scope.viewModel = angular.copy(model);
            else
                $scope.viewModel = { ID: 0 };

            $uibModal.open({
                templateUrl: 'js/app/division/divisionViewModal.html',
                controller: 'divisionViewModalCtrl',
                scope: $scope,
                backdrop: 'static',
                keyboard: false
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        $scope.search();
    }

})(angular.module('ilsApp'));