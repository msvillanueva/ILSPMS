(function (app) {
    'use strict';

    app.controller('dashboardCtrl', dashboardCtrl);
    dashboardCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function dashboardCtrl($scope, apiService, notificationService) {
        $scope.projects = [];
        $scope.closedProjectData = {};
        $scope.approvals = 0;
        $scope.forSubmit = 0;
        $scope.movements = ["1", "2", "3"];
        $scope.totalMovements = 0;
        $scope.totalActivities = 0;

        //dougnut chart
        $scope.cdColors = ["#84ffff", "#ff8a80"];
        $scope.cdLabels = [];
        $scope.cdData = [];
        $scope.cdOptions = { legend: { display: true } };

        function init() {
            if ($scope.isAdmin) {
                showAdmin();
            }
            else {
                showUser();
            }
        }

        function showAdmin() {

        }

        function showUser() {
            $scope.loadingData = true;
            apiService.get('/api/dashboard/', null,
                loadComplete,
                notificationService.responseFailed);
        }

        function loadComplete(response) {
            $scope.loadingData = false;
            console.log(response);
            $scope.approvals = response.data.approval;
            $scope.projects = response.data.projects;
            $scope.closedProjectData = response.data.closeProject;
            $scope.movements = response.data.movements;
            $scope.totalMovements = response.data.totalMovements;
            $scope.totalActivities = response.data.totalActivities;
            
            if ($scope.isPM || $scope.isDiv)
                loadCanSubmit();

            $scope.cdLabels = response.data.closeProject.Labels;
            $scope.cdData = response.data.closeProject.Items;
        }

        function loadCanSubmit() {
            angular.forEach($scope.projects, function (proj, key) {
                if (!proj.LockSubmit && proj.ProjectManagerID == $scope.userData.id)
                    $scope.forSubmit++;
            });
        }

        init();
    }

})(angular.module('ilsApp'));
