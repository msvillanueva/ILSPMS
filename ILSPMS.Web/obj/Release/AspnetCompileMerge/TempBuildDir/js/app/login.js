(function () {
    'use strict';

    angular.module('loginApp', ['base64', 'ngCookies', 'common.core', 'common.ui', 'ngRoute'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider', '$locationProvider'];
    function config($routeProvider, $locationProvider, $scope) {
        $routeProvider
            .when("/", {
                title: 'Login',
                templateUrl: "js/app/login/loginForm.html",
                controller: "loginFormCtrl"
            })
            .when("/forgotPassword", {
                title: 'Forgot Password',
                templateUrl: "js/app/login/forgotPassword.html",
                controller: "forgotPasswordCtrl"
            })
            .otherwise({ redirectTo: "/" });

    }

    run.$inject = ['$rootScope', '$location', '$cookieStore', '$http', 'membershipService', '$route'];
    function run($rootScope, $location, $cookieStore, $http, membershipService, $route) {
        $rootScope.repository = $cookieStore.get('ilsPMSrepo') || {};
        if (membershipService.isUserLoggedIn() && $rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authdata;
            $http.defaults.headers.common['Roles'] = $rootScope.repository.loggedUser.role;
            window.location.href = 'index.html';
        }
        else {
            membershipService.removeCredentials();
        }
    }


})();