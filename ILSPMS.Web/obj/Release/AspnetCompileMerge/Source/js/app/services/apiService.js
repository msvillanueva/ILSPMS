(function (app) {
    'use strict';

    app.factory('apiService', apiService);

    apiService.$inject = ['$http', '$location', 'notificationService', '$rootScope', '$interval'];

    function apiService($http, $location, notificationService, $rootScope, $interval) {
        var service = {
            get: get,
            post: post,
            silentGet: silentGet,
            silentPost: silentPost
        };

        function get(url, config, success, failure) {
            return $http.get(url, config)
                    .then(function (result) {
                        success(result);
                    }, function (error) {
                        if (error.status == '401') {
                            notificationService.displayError('Authentication required.');
                            errorAction();
                        }
                        else if (failure != null) {
                            failure(error);
                        }
                    });
        }

        function silentGet(url, config, success, failure) {
            return $http.get(url, { ignoreLoadingBar: true })
                    .then(function (result) {
                        success(result);
                    }, function (error) {
                        if (error.status == '401') {
                            notificationService.displayError('Authentication required.');
                            errorAction();
                        }
                        else if (failure != null) {
                            failure(error);
                        }
                    });
        }

        function post(url, data, success, failure) {
            return $http.post(url, data)
                    .then(function (result) {
                        success(result);
                    }, function (error) {
                        if (error.status == '401') {
                            notificationService.displayError('Authentication required.');
                            errorAction();
                        }
                        else if (failure != null) {
                            failure(error);
                        }
                    });
        }

        function silentPost(url, data, success, failure) {
            return $http.post(url, data, { ignoreLoadingBar: true })
                    .then(function (result) {
                        success(result);
                    }, function (error) {
                        if (error.status == '401') {
                            notificationService.displayError('Authentication required.');
                            errorAction();
                        }
                        else if (failure != null) {
                            failure(error);
                        }
                    });
        }

        function errorAction() {
            $rootScope.previousState = $location.path();
            membershipService.removeCredentials();
            $interval(function () { window.location.href = 'login.html'; }, 3000);
        }

        return service;
    }

})(angular.module('common.core'));