(function (app) {
    'use strict';

    app.controller('userViewModalCtrl', userViewModalCtrl);

    userViewModalCtrl.$inject = ['$scope', '$uibModalInstance', '$timeout', 'apiService', 'notificationService'];

    function userViewModalCtrl($scope, $uibModalInstance, $timeout, apiService, notificationService) {
        $scope.close = close;
        $scope.save = save;
        
        function save() {
            var url = '';
            if ($scope.viewModel.ID > 0)
                url = '/api/users/edit/';
            else
                url = '/api/users/new/';
            
            $scope.dataLoading = true;
            apiService.post(
                url,
                $scope.viewModel,
                saveCompleted,
                notificationService.responseFailed
            );
        }

        function saveCompleted(response) {
            $scope.dataLoading = false;
            if (response.data.success) {

                if ($scope.viewModel.ID == 0) {
                    notificationService.displaySuccess('New user has been added successfully.');
                    $scope.users.push(angular.copy(response.data.item));
                    $scope.viewModel = {};
                }
                else {
                    notificationService.displaySuccess('User account has been updated successfully.');
                    updateRow(response.data.item);
                }
                $uibModalInstance.dismiss();
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function updateRow(item) {
            angular.forEach($scope.users, function (row, key) {
                if (row.ID == $scope.viewModel.ID) {
                    row.FirstName = item.FirstName;
                    row.LastName = item.LastName;
                    row.FullName = item.FullName;
                    row.Email = item.Email;
                    row.DivisionName = item.DivisionName;
                    row.RoleName = item.RoleName;
                    row.RoleID = item.RoleID;
                    row.DivisionID = item.DivisionID;
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