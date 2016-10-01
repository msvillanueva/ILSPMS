(function (app) {
    'use strict';

    app.controller('profileCtrl', profileCtrl);

    profileCtrl.$inject = ['$scope', '$timeout', 'apiService', 'notificationService'];

    function profileCtrl($scope, $timeout, apiService, notificationService) {
        $scope.close = close;
        $scope.save = save;

        function save() {
            $scope.dataLoading = true;
            apiService.post(
                '/api/account/update/',
                $scope.viewModel,
                saveCompleted,
                notificationService.responseFailed
            );
        }

        function saveCompleted(response) {
            $scope.dataLoading = false;
            if (response.data.success) {
                notificationService.displaySuccess('Your profile has been updated successfully');
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function init() {
            apiService.get('/api/account',
                null,
                function (response) {
                    $scope.dataLoading = false;
                    $scope.viewModel = response.data.item;
                    $scope.availableUsername = $scope.viewModel.Username != '';
                },
                notificationService.responseFailed
            );
        }

        init();
    }

})(angular.module('ilsApp'));