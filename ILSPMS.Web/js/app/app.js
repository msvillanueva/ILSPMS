(function () {
    'use strict';

    angular.module('ilsApp', ['common.core', 'common.ui'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider', '$locationProvider'];
    function config($routeProvider, $locationProvider, $scope) {
        $routeProvider
            .when("/", {
                title: 'Dashboard',
                templateUrl: "js/app/dashboard/dashboard.html",
                controller: "dashboardCtrl",
                menu: '#menuDashboard'
            })
            .when("/profile", {
                title: 'Profile',
                templateUrl: "js/app/profile/profile.html",
                controller: "profileCtrl",
                menu: '#menuDashboard'
            })
            .when("/change-password", {
                title: 'Change password',
                templateUrl: "js/app/profile/password.html",
                controller: "passwordCtrl",
                menu: '#menuDashboard'
            })
            .when("/users", {
                title: 'Users Management',
                templateUrl: "js/app/user/users.html",
                controller: "usersCtrl",
                menu: '#menuUsers',
                resolve: { allowed: isLoggedAdmin }
            })
            .when("/divisions", {
                title: 'Divisions Management',
                templateUrl: "js/app/division/divisions.html",
                controller: "divisionsCtrl",
                menu: '#menuDivisions',
                resolve: { allowed: isLoggedAdmin }
            })
            .when("/projects", {
                title: 'Projects Management',
                templateUrl: "js/app/project/projects.html",
                controller: "projectsCtrl",
                menu: '#menuProjects',
                resolve: { allowed: isLoggedAdmin }
            })
            .when("/my-projects", {
                title: 'My Projects',
                templateUrl: "js/app/myproject/myProjects.html",
                controller: "myProjectsCtrl",
                menu: '#menuMyProjects',
                resolve: { allowed: isLoggedNotAdmin }
            })
            .when("/my-approvals", {
                title: 'Projects for Approval',
                templateUrl: "js/app/myproject/myProjects.html",
                controller: "myApprovalsCtrl",
                menu: '#menuMyApprovals',
                resolve: { allowed: isLoggedNotAdminNotPM }
            })
            .when("/project-movements/:id", {
                title: 'My Projects',
                templateUrl: "js/app/movement/movements.html",
                controller: "movementsCtrl",
                menu: '#menuMyProjects',
                resolve: { allowed: isLoggedNotAdmin }
            })
            .when("/project-activities/:id", {
                title: 'My Projects',
                templateUrl: "js/app/activity/activities.html",
                controller: "activitiesCtrl",
                menu: '#menuMyProjects',
                resolve: { allowed: isLoggedNotAdmin }
            })
            .otherwise({ redirectTo: "/" });

    }

    run.$inject = ['$rootScope', '$location', '$cookieStore', '$http', 'membershipService', '$route'];

    function run($rootScope, $location, $cookieStore, $http, membershipService, $route) {
        // handle page refreshes

        // credentials
        $rootScope.repository = $cookieStore.get('ilsPMSrepo') || {};
        if (!membershipService.isUserLoggedIn())
            window.location.href = 'login.html';

        if ($rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authdata;
            $http.defaults.headers.common['Roles'] = $rootScope.repository.loggedUser.role;
        }

        // route
        $rootScope.$on('$routeChangeSuccess', function () {
            document.title = 'PLMS | ' + $route.current.title;
            var subLogo = document.getElementsByClassName('sub-logo')[0];
            if (subLogo)
                document.getElementsByClassName('sub-logo')[0].innerHTML = ' | ' + $route.current.title;

            var liMenu = document.getElementsByClassName('menu-item');
            angular.forEach(liMenu, function (li, key) {
                angular.element(li).removeClass('active');
            });

            var refElem = angular.element(document.querySelector($route.current.menu));
            if (refElem && refElem.context && refElem.context.nodeName == 'LI') {
                refElem.addClass('active');
            }
        });

        $(document).ready(function () {
            $(".fancybox").fancybox({
                openEffect: 'none',
                closeEffect: 'none'
            });

            $('.fancybox-media').fancybox({
                openEffect: 'none',
                closeEffect: 'none',
                helpers: {
                    media: {}
                }
            });

            $('[data-toggle=offcanvas]').click(function () {
                $('.row-offcanvas').toggleClass('active');
            });

        });
    }

    isAuthenticated.$inject = ['membershipService', '$rootScope', '$location'];
    function isAuthenticated(membershipService, $rootScope, $location) {
        if (!membershipService.isUserLoggedIn()) {
            $rootScope.previousState = $location.path();
            window.location.href = '/';
        }
    }

    var isLoggedAdmin = ['membershipService', '$rootScope', '$location', '$q', function (membershipService, $rootScope, $location, $q) {
        var deferred = $q.defer();
        if ($rootScope.repository.loggedUser.role != 1) {
            return deferred.promise;
        }
    }];

    var isLoggedNotAdmin = ['membershipService', '$rootScope', '$location', '$q', function (membershipService, $rootScope, $location, $q) {
        var deferred = $q.defer();
        if ($rootScope.repository.loggedUser.role == 1) {
            return deferred.promise;
        }
    }];

    var isLoggedNotAdminNotPM = ['membershipService', '$rootScope', '$location', '$q', function (membershipService, $rootScope, $location, $q) {
        var deferred = $q.defer();
        if ($rootScope.repository.loggedUser.role == 1 || $rootScope.repository.loggedUser.role == 2) {
            return deferred.promise;
        }
    }];

})();