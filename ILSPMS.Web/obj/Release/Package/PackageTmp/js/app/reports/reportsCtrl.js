(function (app) {
    'use strict';

    app.controller('reportsCtrl', reportsCtrl);
    reportsCtrl.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function reportsCtrl($scope, apiService, notificationService, $filter) {
        $scope.pageClass = 'page-reports';
        $scope.loadingData = true;
        $scope.divisions = [];
        $scope.divisionID = '0';
        $scope.years = [];
        $scope.selectedYear = $scope.year.toString();
        $scope.data = [];
        $scope.topMilestone = 0;

        $scope.generate = generate;
        $scope.downloadPdf = downloadPdf;

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

        function downloadPdf() {
            var pdfContent = [];

            pdfContent.push({ text: 'ILS - Project Life-cycle Monitoring System', style: 'pageHeader' });
            pdfContent.push({ text: 'Date Generated: ' + $filter('date')((new Date()), "MMM dd, yyyy"), style: 'pageSubHeader' });

            angular.forEach($scope.data, function (div, key) {
                pdfContent.push({ text: div.Division.Name, style: 'divHeader' });
                pdfContent.push({ text: 'Budget: Php ' + $filter('number')(div.TotalBudgetAllocated,2) + '/' + $filter('number')(div.TotalBudget,2), style: 'reportTable' });
                pdfContent.push({ text: 'Utilized: ' + $filter('number')(div.BudgetUtilized, 2) + '%', style: 'reportTable' });

                var tblContent = {};
                tblContent.style = 'reportTable';

                var rptTable = {};
                rptTable.widths = ['*', '*', '*', '*', '*', '*'];
                var rptBody = [];
                rptBody.push([
                    { text: 'Project Name', style: 'header' },
                    { text: 'PM', style: 'header' },
                    { text: 'Budget (Php)', style: 'header' },
                    { text: 'Utilized (Php)', style: 'header' },
                    { text: '% Utilized', style: 'header' },
                    { text: '% Completion', style: 'header' }
                ]);

                angular.forEach(div.Projects, function (proj, pKey) {
                    rptBody.push([
                        proj.Name,
                        proj.ProjectManager,
                        $filter('number')(proj.Budget, 2),
                        $filter('number')(proj.BudgetUtilized, 2),
                        $filter('number')((proj.BudgetUtilized / proj.Budget) * 100, 2),
                        $filter('number')((proj.MilestoneOrder / $scope.topMilestone), 2)
                    ]);
                });

                rptTable.body = rptBody;

                tblContent.table = rptTable;
                pdfContent.push(tblContent);
                pdfContent.push({ text: '' });
                pdfContent.push({ text: '' });
            });


            var docDefinition = {
                content: pdfContent,
                styles: {
                    header: {
                        bold: true,
                        color: '#000',
                        fontSize: 11
                    },
                    reportTable: {
                        color: '#666',
                        fontSize: 10
                    },
                    pageHeader: {
                        fontSize: 18,
                        bold: true
                    },
                    pageSubHeader: {
                        fontSize: 16,
                        color:'#333'
                    },
                    divHeader: {
                        fontSize: 14,
                        bold: true,
                        margin: [0, 30, 0, 0]
                    }
                }
            };
            pdfMake.createPdf(docDefinition).download('PLMS-report.pdf');
        }

        init();
    }

})(angular.module('ilsApp'));