(function (app) {
    'use strict';

    app.controller('activityViewModalCtrl', activityViewModalCtrl);

    activityViewModalCtrl.$inject = ['$scope', '$uibModalInstance', '$timeout', 'apiService', 'notificationService'];

    function activityViewModalCtrl($scope, $uibModalInstance, $timeout, apiService, notificationService) {
        $scope.close = close;
        $scope.save = save;

        function save() {
            $scope.dataLoading = true;
            apiService.post(
                '/api/activities/update/',
                $scope.viewModel,
                saveCompleted,
                notificationService.responseFailed
            );
        }

        function saveCompleted(response) {
            $scope.dataLoading = false;
            if (response.data.success) {
                if ($scope.viewModel.ID == 0) {
                    notificationService.displaySuccess('New activity has been added successfully.');
                }
                else {
                    notificationService.displaySuccess('Activity has been updated successfully.');
                }
                $scope.init();
                $uibModalInstance.dismiss();
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function close() {
            $scope.isEnabled = false;
            $uibModalInstance.dismiss();
        }
    }

})(angular.module('ilsApp'));