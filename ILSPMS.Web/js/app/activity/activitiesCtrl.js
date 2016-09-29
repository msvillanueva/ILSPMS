(function (app) {
    'use strict';

    app.controller('activitiesCtrl', activitiesCtrl);

    activitiesCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$routeParams'];

    function activitiesCtrl($scope, apiService, notificationService, $routeParams) {
        $scope.dataLoading = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.milestones = [];
        $scope.activities = [];
        $scope.tableRowCollection = [];
        $scope.lastMilestoneID = 0;
        $scope.canAdd = false;

        $scope.showActivities = showActivities;

        function init() {
            var config = {
                params: {
                    id: $routeParams.id
                }
            };

            apiService.get('/api/activities',
                config,
                loadCompleted,
                notificationService.responseFailed
            );
        }

        function loadCompleted(response) {
            if (response.data.success) {
                $scope.dataLoading = false;
                $scope.milestones = response.data.items;
                $scope.showActivities($scope.milestones[response.data.items.length - 1]);
                $scope.lastMilestoneID = $scope.milestones[response.data.items.length - 1].MilestoneID;
                $scope.setTitle(response.data.title + ' | Activities');
                $scope.canAdd = true;
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function showActivities(row) {
            $scope.tableRowCollection = row.Movements;
            $scope.projects = [].concat($scope.tableRowCollection);

            var cards = angular.element(document.querySelectorAll('.milestone-card'));
            cards.removeClass('selected');

            var card = angular.element(document.querySelector('#card' + row.MilestoneID));
            card.addClass('selected');
            $scope.canAdd = $scope.lastMilestoneID == row.MilestoneID;
        }

        init();
    }

})(angular.module('ilsApp'));