(function() {
    var module = angular.module("logViewer");
    module.controller("testController", ["logViewerService",
        "$scope", function (logViewerService, scope) {
            scope.add = function() {
                logViewerService.send({
                    message: prompt("What'cha wanna say?", ""),
                    level: scope.level
                });
            };

            scope.levels = [
                "Info",
                "Debug",
                "Error"
            ];
            scope.level = "Info";

            scope.test = "Halluh!";
        }
    ]);
}());