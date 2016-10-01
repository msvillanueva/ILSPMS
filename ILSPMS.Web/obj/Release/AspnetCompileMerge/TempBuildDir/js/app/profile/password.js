(function (app) {
    'use strict';

    app.controller('passwordCtrl', passwordCtrl);

    passwordCtrl.$inject = ['$scope', 'apiService', 'notificationService', 'membershipService', 'cambria'];

    function passwordCtrl($scope, apiService, notificationService, membershipService, cambria) {
        $scope.pageClass = 'page-password';
        $scope.dataLoading = false;
        $scope.viewModel = {};

        $scope.save = save;

        function save() {
            $scope.dataLoading = true;

            var sendPassword = {};
            sendPassword.Current = membershipService.encStr($scope.viewModel.Current);
            sendPassword.New = membershipService.encStr($scope.viewModel.New);
            sendPassword.Confirm = membershipService.encStr($scope.viewModel.Confirm);
            apiService.post('/api/account/changepassword/',
                sendPassword,
                updateSuccess,
                notificationService.responseFailed
            );
        }

        function updateSuccess(response) {
            $scope.dataLoading = false;
            if (response.data.success) {
                cambria.cAlert('Password was updated. <br/> You will be redirected to the login screen', 'PASSWORD UPDATED', function () {
                    membershipService.removeCredentials();
                    window.location.href = 'login.html';
                });
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }
    }

})(angular.module('ilsApp'));