﻿(function (app) {
    'use strict';

    app.controller('rootCtrl', rootCtrl);

    rootCtrl.$inject = ['$scope', '$location', 'membershipService', '$rootScope', 'apiService', '$interval', 'notificationService'];
    function rootCtrl($scope, $location, membershipService, $rootScope, apiService, $interval, notificationService) {
        $scope.userData = {};
        $scope.UserEntity = {};
        $scope.Alerts = [];
        $scope.itemsByPage = 10;
        $scope.role = 0;
        $scope.username = '';
        $scope.isAdmin = false;

        $scope.displayUserInfo = displayUserInfo;
        $scope.logout = logout;
        $scope.formatDate = formatDate;

        function displayUserInfo() {
            $scope.userData.isUserLoggedIn = membershipService.isUserLoggedIn();
            if ($scope.userData.isUserLoggedIn) {
                $scope.username = $rootScope.repository.loggedUser.username;
                $scope.role = $rootScope.repository.loggedUser.role;
                $scope.userData.id = $rootScope.repository.loggedUser.id;
                $scope.isAdmin = $rootScope.repository.loggedUser.role == 1;
            }
            else {
                logout();
            }
        }

        function logout() {
            membershipService.removeCredentials();
            window.location.href = 'login.html';
        }

        function formatDate(date) {
            var dateOut = new Date(date);
            return dateOut;
        };

        $scope.displayUserInfo();
    }

})(angular.module('ilsApp'));
