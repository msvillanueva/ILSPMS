(function (app) {
    'use strict';

    app.controller('reportsCtrl', reportsCtrl);
    reportsCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function reportsCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-reports';
        $scope.loadingData = true;
        $scope.divisions = [];
        $scope.divisionID = '0';
        $scope.years = [];
        $scope.selectedYear = $scope.year.toString();
        $scope.data = [];
        $scope.topMilestone = 0;

        $scope.generate = generate;

        function init() {
            if ($scope.isDirector) {
                apiService.get('/api/divisions/', null,
                    function (result) {
                        $scope.divisions = result.data.items;
                    },
                    notificationService.responseFailed);
            }

            apiService.post('/api/projects/years', null,
                function (result) {
                    $scope.years = result.data.items;
                },
                notificationService.responseFailed);

            $scope.generate();
        }

        function generate() {
            $scope.loadingData = true;
            var config = {
                params: {
                    divisionID: $scope.divisionID,
                    year: $scope.selectedYear
                }
            };

            apiService.get('/api/reportdata/',
                config,
                dataLoaded,
                notificationService.responseFailed);
        }

        function dataLoaded(response) {
            $scope.loadingData = false;
            $scope.data = response.data.items;
            $scope.topMilestone = response.data.ms;
        }

        init();
    }

})(angular.module('ilsApp'));