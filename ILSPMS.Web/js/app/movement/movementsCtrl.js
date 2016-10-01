(function (app) {
    'use strict';

    app.controller('movementsCtrl', movementsCtrl);

    movementsCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$routeParams'];

    function movementsCtrl($scope, apiService, notificationService, $routeParams) {
        $scope.pageClass = 'page-users';
        $scope.dataLoading = true;
        $scope.page = 0;
        $scope.pagesCount = 0;

        $scope.milestones = [];

        $scope.movements = [];
        $scope.tableRowCollection = [];

        $scope.showMovements = showMovements;

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
                $scope.showMovements($scope.milestones[response.data.items.length - 1]);
                $scope.setTitle(response.data.title + ' | Movements');
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function showMovements(row) {
            console.log(row);
            $scope.tableRowCollection = row.Movements;
            $scope.movements = [].concat($scope.tableRowCollection);

            var cards = angular.element(document.querySelectorAll('.milestone-card'));
            cards.removeClass('selected');

            var card = angular.element(document.querySelector('#card' + row.MilestoneID));
            card.addClass('selected');
        }

        init();
    }

})(angular.module('ilsApp'));