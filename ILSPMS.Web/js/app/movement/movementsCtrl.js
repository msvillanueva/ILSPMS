(function (app) {
    'use strict';

    app.controller('movementsCtrl', movementsCtrl);

    movementsCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$routeParams'];

    function movementsCtrl($scope, apiService, notificationService, $routeParams) {
        $scope.milestones = [];

        function init() {
            var config = {
                params: {
                    id: $routeParams.id
                }
            };

            apiService.get('/api/movements',
                config,
                loadCompleted,
                notificationService.responseFailed
            );
        }

        function loadCompleted(response) {
            if (response.data.success) {
                $scope.dataLoading = false;
                $scope.milestones = response.data.items;
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        init();
    }

})(angular.module('ilsApp'));