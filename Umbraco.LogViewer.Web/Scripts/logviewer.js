(function() {
    "use strict";

    var logViewer;

    function LogViewerController($http, $scope) {
        function listen() {
            $http({ method: 'GET', url: '/LogViewer/LogEntries', timeout: 5000 })
                .success(function (data, status, headers, config) {
                    $scope.greeting = data.data;
                    listen();
                })
                .error(function (data, status, headers, config) {
                    if (status === 0) {
                        listen();
                        return;
                    }

                    $scope.greeting = "error";
                });
        }

        $scope.data = "test";

        $scope.setData = function() {
            $http({ method: 'POST', url: '/LogViewer/Set', data: { data: $scope.data } });
            $scope.data = "";
        };

        listen();
    }

    function main() {
        logViewer = angular.module("logViewer", []);
        logViewer.controller("logViewerController", ["$http", "$scope", LogViewerController]);
    }

    main();

}());