(function (app) {
    'use strict';

    app.controller('uploadModalCtrl', uploadModalCtrl);

    uploadModalCtrl.$inject = ['$scope', '$uibModalInstance', '$timeout', 'apiService', 'notificationService', 'fileUploadService'];

    function uploadModalCtrl($scope, $uibModalInstance, $timeout, apiService, notificationService, fileUploadService) {
        $scope.close = close;
        $scope.save = save;
        $scope.prepareFiles = prepareFiles;

        var uploadImage = null;

        function save() {
            if (uploadImage) {

                $scope.viewModel.ProjectActivityID = $scope.activity.ID;

                $scope.dataLoading = true;
                apiService.post(
                    '/api/files/save/',
                    $scope.viewModel,
                    saveCompleted,
                    notificationService.responseFailed
                );
            }
            else {
                notificationService.displayError('File attachment is required');
            }
        }

        function saveCompleted(response) {
            $scope.dataLoading = false;
            if (response.data.success) {

                fileUploadService.uploadFile(uploadImage, response.data.id, function () {
                    $scope.init();
                });

                uploadImage = null;
                $scope.viewModel = {};
                $uibModalInstance.dismiss();
            }
            else {
                notificationService.displayError(response.data.message);
            }
        }

        function close() {
            $scope.isEnabled = false;
            $uibModalInstance.dismiss();
        }

        function prepareFiles($files) {
            uploadImage = $files;
        }

    }

})(angular.module('ilsApp'));