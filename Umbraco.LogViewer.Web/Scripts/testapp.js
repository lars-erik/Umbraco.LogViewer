(function() {
    var module = angular.module("logViewer");
    module.controller("testController", ["logViewerService",
        "$scope", function (logViewerService, scope) {
            scope.add = function() {
                logViewerService.send(prompt("What'cha wanna say?", ""));
            };

            scope.test = "Halluh!";
        }
    ]);
}());