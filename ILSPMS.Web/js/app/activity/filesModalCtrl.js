(function (app) {
    'use strict';

    app.controller('filesModalCtrl', filesModalCtrl);

    filesModalCtrl.$inject = ['$scope', '$uibModalInstance', '$timeout', 'apiService', 'notificationService', 'cambria'];

    function filesModalCtrl($scope, $uibModalInstance, $timeout, apiService, notificationService, cambria) {
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.tableRowCollection = [];
        $scope.files = [];

        $scope.close = close;
        $scope.archiveFile = archiveFile;

        function init() {
            $scope.tableRowCollection = $scope.activity.Files;
            $scope.files = [].concat($scope.tableRowCollection);
        }

        function close() {
            $scope.isEnabled = false;
            $uibModalInstance.dismiss();
        }

        function archiveFile(idx, row) {
            cambria.cConfirm('Are you sure you want to remove this file<br/> <strong>permanently</strong> from the server?', 'CONFIRM ACTION', function (click) {
                if (click) {
                    $scope.files.splice(idx, 1);
                    apiService.post(
                        '/api/files/remove/',
                        row,
                        function (response) {
                            if (response.data.success)
                                notificationService.displaySuccess(row.Name + ' was permanently removed from the server');
                            else
                                notificationService.displayError(response.data.message);
                        },
                        notificationService.responseFailed
                    );
                }
            });
        }

        init();

    }

})(angular.module('ilsApp'));