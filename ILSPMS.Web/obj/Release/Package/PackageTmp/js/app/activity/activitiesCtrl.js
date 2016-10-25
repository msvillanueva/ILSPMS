(function (app) {
    'use strict';

    app.controller('activitiesCtrl', activitiesCtrl);

    activitiesCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$routeParams', '$uibModal', 'cambria'];

    function activitiesCtrl($scope, apiService, notificationService, $routeParams, $uibModal, cambria) {
        $scope.dataLoading = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.milestones = [];
        $scope.activities = [];
        $scope.tableRowCollection = [];
        $scope.lastMilestoneID = 0;
        $scope.canAdd = false;
        $scope.owned = false;
        $scope.IsSubmitted = false;

        $scope.showActivities = showActivities;
        $scope.init = init;
        $scope.create = create;
        $scope.archive = archive;
        $scope.upload = upload;
        $scope.files = files;

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
                $scope.setTitle(response.data.project.Name + ' | Activities');
                $scope.owned = $scope.userData.id == response.data.project.ProjectManagerID;
                $scope.canAdd = $scope.owned;
                $scope.IsSubmitted = response.data.project.LockSubmit;
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function showActivities(row) {
            $scope.tableRowCollection = row.Activities;
            $scope.activities = [].concat($scope.tableRowCollection);

            var cards = angular.element(document.querySelectorAll('.milestone-card'));
            cards.removeClass('selected');

            var card = angular.element(document.querySelector('#card' + row.MilestoneID));
            card.addClass('selected');
            $scope.canAdd = $scope.lastMilestoneID == row.MilestoneID && $scope.owned;
        }

        function create(model) {
            if (model)
                $scope.viewModel = angular.copy(model);
            else
                $scope.viewModel = { ID: 0, BudgetUtilized: 0, MilestoneID: $scope.lastMilestoneID, ProjectID: $routeParams.id };

            $uibModal.open({
                templateUrl: 'js/app/activity/activityViewModal.html',
                controller: 'activityViewModalCtrl',
                scope: $scope,
                backdrop: 'static',
                keyboard: false
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function upload(model) {
            $scope.activity = angular.copy(model);
            $uibModal.open({
                templateUrl: 'js/app/activity/uploadModal.html',
                controller: 'uploadModalCtrl',
                scope: $scope,
                backdrop: 'static',
                keyboard: false
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function files(model) {
            $scope.activity = angular.copy(model);
            $uibModal.open({
                templateUrl: 'js/app/activity/filesModal.html',
                controller: 'filesModalCtrl',
                scope: $scope,
                backdrop: 'static',
                keyboard: false
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function archive(idx, row) {
            cambria.cConfirm('Remove this activity?', 'CONFIRM ACTION', function (click) {
                if (click) {
                    $scope.activities.splice(idx, 1);
                    apiService.post(
                        '/api/activities/remove/',
                        row,
                        function (response) {
                            if (response.data.success)
                                notificationService.displaySuccess(row.Activity + ' was removed from the list.');
                            else
                                notificationService.displayError(response.data.message);
                        },
                        notificationService.responseFailed
                    );
                }
            });
        }

        $scope.init();
    }

})(angular.module('ilsApp'));