(function (app) {
    'use strict';

    app.controller('loginFormCtrl', ['$scope', '$rootScope', '$location', 'membershipService', 'notificationService',
            function ($scope, $rootScope, $location, membershipService, notificationService) {
                $scope.user = {};
                $scope.login = function () {
                    membershipService.removeCredentials();
                    $scope.dataLoading = true;
                    membershipService.login($scope.user, function (response) {
                        if (response.success) {
                            membershipService.saveCredentials($scope.user, response.role, response.id);
                            window.location.href = 'index.html';
                        } else {
                            notificationService.displayError('Invalid Login');
                            $scope.user.password = '';
                            $scope.error = response.message;
                            $scope.dataLoading = false;
                        }
                    });
                };

            }]);

})(angular.module('loginApp'));


