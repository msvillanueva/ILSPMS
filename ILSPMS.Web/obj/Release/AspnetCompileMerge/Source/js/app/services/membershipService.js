(function (app) {
    'use strict';

    app.factory('membershipService', membershipService);

    membershipService.$inject = ['apiService', 'notificationService', '$http', '$base64', '$cookieStore', '$rootScope'];

    function membershipService(apiService, notificationService, $http, $base64, $cookieStore, $rootScope) {

        var service = {
            login: login,
            saveCredentials: saveCredentials,
            removeCredentials: removeCredentials,
            isUserLoggedIn: isUserLoggedIn,
            encStr: encStr
        }

        function login(user, callback) {
            $http.post('/api/account/authenticate', user)
                .success(function (response) {
                    callback(response);
                });
        }

        function saveCredentials(user, role, id) {
            var membershipData = $base64.encode(user.username + ':' + user.password);
            $rootScope.repository = {
                loggedUser: {
                    username: user.username,
                    authdata: membershipData,
                    role: role,
                    id: id,
                    impersonating: {},
                    isImpersonated: false
                }
            };
            $http.defaults.headers.common['Authorization'] = 'Basic ' + membershipData;
            $cookieStore.put('ilsPMSrepo', $rootScope.repository);
        }

        function removeCredentials() {
            $rootScope.repository = {};
            $cookieStore.remove('ilsPMSrepo');
            $http.defaults.headers.common['Authorization'] = '';
            $http.defaults.headers.common['Roles'] = '';
        };

        function loginFailed(response) {
            notificationService.displayError(response.data);
        }

        function isUserLoggedIn() {
            return $rootScope.repository != null && $rootScope.repository.loggedUser != null;
        }

        function encStr(_val) {
            return $base64.encode(_val);
        }

        return service;
    }

})(angular.module('common.core'));