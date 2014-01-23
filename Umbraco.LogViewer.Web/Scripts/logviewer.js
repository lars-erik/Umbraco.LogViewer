(function() {
    "use strict";

    var logViewer;

    function LogViewerController(logViewerService, $scope) {
        $scope.messages = ["test", "tast"];

        logViewerService.logged = function(data) {
            $scope.$apply(function() {
                $scope.messages.push(data);
            });
        };
    }

    function LogViewerService() {
        var appender = $.connection("/signalrappender"),
            isConnected = false,
            self = this;

        this.send = function(data) {
            appender.send(data);
        };

        appender.received(function(data) {
            if (self.logged) {
                self.logged(data);
            }
        });

        appender.start().done(function() {
            isConnected = true;
        });
    }

    function main() {
        logViewer = angular.module("logViewer", []);
        logViewer.controller("logViewerController", ["logViewerService", "$scope", LogViewerController]);
        logViewer.service("logViewerService", [LogViewerService]);
    }

    main();

}());