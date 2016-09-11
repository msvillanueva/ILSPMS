(function (app) {
    'use strict';

    app.controller('divisionViewModalCtrl', divisionViewModalCtrl);

    divisionViewModalCtrl.$inject = ['$scope', '$uibModalInstance', '$timeout', 'apiService', 'notificationService'];

    function divisionViewModalCtrl($scope, $uibModalInstance, $timeout, apiService, notificationService) {
        $scope.close = close;
        $scope.save = save;

        function save() {
            $scope.dataLoading = true;
            apiService.post(
                '/api/divisions/update/',
                $scope.viewModel,
                saveCompleted,
                notificationService.responseFailed
            );
        }

        function saveCompleted(response) {
            $scope.dataLoading = false;
            if (response.data.success) {

                if ($scope.viewModel.ID == 0) {
                    notificationService.displaySuccess('New division has been added successfully.');
                    $scope.divisions.push(angular.copy(response.data.item));
                }
                else {
                    notificationService.displaySuccess('Division has been updated successfully.');
                    updateRow();
                }
                $uibModalInstance.dismiss();
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function updateRow() {
            angular.forEach($scope.divisions, function (row, key) {
                if (row.ID == $scope.viewModel.ID) {
                    row.Name = $scope.viewModel.Name;
                }
            });
        }

        function close() {
            $scope.isEnabled = false;
            $uibModalInstance.dismiss();
        }
    }

})(angular.module('ilsApp'));