(function (app) {
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
        $scope.isPM = false;
        $scope.isDiv = false;
        $scope.year = (new Date()).getFullYear();

        $scope.displayUserInfo = displayUserInfo;
        $scope.logout = logout;
        $scope.formatDate = formatDate;
        $scope.filterValue = filterValue;

        function displayUserInfo() {
            $scope.userData.isUserLoggedIn = membershipService.isUserLoggedIn();
            if ($scope.userData.isUserLoggedIn) {
                $scope.username = $rootScope.repository.loggedUser.username;
                $scope.role = $rootScope.repository.loggedUser.role;
                $scope.userData.id = $rootScope.repository.loggedUser.id;
                $scope.isAdmin = $rootScope.repository.loggedUser.role == 1;
                $scope.isPM = $rootScope.repository.loggedUser.role == 2;
                $scope.isPM = $rootScope.repository.loggedUser.role == 3;
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

        function filterValue($event) {
            if (isNaN(String.fromCharCode($event.keyCode))) {
                $event.preventDefault();
            }
        };

        $scope.displayUserInfo();
    }

})(angular.module('ilsApp'));
