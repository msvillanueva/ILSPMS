(function (app) {
    'use strict';

    app.controller('projectViewModalCtrl', projectViewModalCtrl);

    projectViewModalCtrl.$inject = ['$scope', '$uibModalInstance', '$timeout', 'apiService', 'notificationService'];

    function projectViewModalCtrl($scope, $uibModalInstance, $timeout, apiService, notificationService) {
        $scope.close = close;
        $scope.save = save;

        function save() {
            var url = '';

            $scope.dataLoading = true;
            apiService.post(
                '/api/projects/update/',
                $scope.viewModel,
                saveCompleted,
                notificationService.responseFailed
            );
        }

        function saveCompleted(response) {
            $scope.dataLoading = false;
            if (response.data.success) {
                if ($scope.viewModel.ID == 0) {
                    notificationService.displaySuccess('New project has been added successfully.');
                    $scope.projects.push(angular.copy(response.data.item));
                    $scope.viewModel = {};
                }
                else {
                    notificationService.displaySuccess('Project has been updated successfully.');
                    updateRow(response.data.item);
                }
                $uibModalInstance.dismiss();
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function updateRow(item) {
            angular.forEach($scope.projects, function (row, key) {
                if (row.ID == $scope.viewModel.ID) {
                    row.Name = item.Name;
                    row.DivisionName = item.DivisionName;
                    row.ProjectManager = item.ProjectManager;
                    row.Budget = item.Budget;
                    row.Year = item.Year;
                }
            });
            $scope.viewModel = {};
        }

        function close() {
            $scope.isEnabled = false;
            $uibModalInstance.dismiss();
        }
    }

})(angular.module('ilsApp'));