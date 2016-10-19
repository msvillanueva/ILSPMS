(function (app) {
    'use strict';

    app.controller('forgotPasswordCtrl', ['$scope', '$rootScope', '$location', 'membershipService', 'notificationService', 'apiService',
            function ($scope, $rootScope, $location, membershipService, notificationService, apiService) {
                $scope.user = {};
                $scope.showCancel = true;
                $scope.requestPassword = function () {
                    $scope.dataLoading = true;
                    $scope.showCancel = false;

                    apiService.post(
                        '/api/account/forgot/',
                        $scope.user,
                        registerCompleted,
                        registerLoadFailed
                    );

                    function registerCompleted(response) {
                        $scope.dataLoading = false;
                        $scope.dataLoading = false;
                        $scope.showCancel = true;
                        if (response.data.success) {
                            notificationService.displaySuccess('Your new password has sent to your email.');
                            $location.path('/');
                        }
                        else {
                            notificationService.displayError(response.data.message);
                        }
                    }

                    function registerLoadFailed(response) {
                        notificationService.displayError('An error occurs while processing your request');
                    }
                };

            }]);

})(angular.module('loginApp'));
