var Umbraco = Umbraco || null;
(function () {
    "use strict";

    var logViewer;

    function LogViewerController(logViewerService, $scope, $http, urls) {
        var loading = [{ id: 0, line: "Loading..." }];

        function nada() {
        }

        function loadLog() {
            $scope.filelog = loading;
            $http.get(urls.filelog)
                .success(function (result) {
                    $scope.filelog = $.map(result.data.split("\n"), function(e, i) {
                        return { id: i, line: e };
                    });
                })
                .error(function() {
                    $scope.filelog = [{ id: 0, line: "crap" }];
                });
        }

        $scope.messages = [];
        $scope.tabs = [
            { name: "Live", "class": "active", activate: nada },
            { name: "File", "class": "", activate: loadLog }
        ];
        $scope.tab = $scope.tabs[0];
        $scope.showSignalR = true;
        $scope.showFile = false;
        $scope.filelog = loading;
        $scope.autoScroll = true;

        $scope.$watch(function() {
            return $scope.tab;
        }, function (newValue, oldValue) {
            oldValue.class = "";
            newValue.class = "active";
            $scope.showSignalR = $scope.tab === $scope.tabs[0];
            $scope.showFile = $scope.tab === $scope.tabs[1];
        });

        $scope.navigate = function(tab) {
            $scope.tab = tab;
            tab.activate();
        };

        $scope.findLevel = function(line) {
            if (line.line.indexOf(" INFO ") > -1) {
                return "info";
            } else if (line.line.indexOf("DEBUG") > -1) {
                return "debug";
            } else if (line.line.indexOf("ERROR") > -1) {
                return "error";
            }
            return "";
        };

        logViewerService.logged = function(data) {
            $scope.$apply(function() {
                $scope.messages.push(data);
            });
        };
    }

    function LogViewerService(urls) {
        var appender = $.connection(urls.connection),
            isConnected = false,
            self = this;

        this.send = function(data) {
            appender.send(JSON.stringify(data));
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
        logViewer = angular.module("umbraco");
        logViewer.factory("logViewer.urls", function () {
            return {
                connection: (Umbraco ? Umbraco.Sys.ServerVariables.umbracoSettings.appPluginsPath : "/App_Plugins") + "/LogViewer/signalrappender",
                filelog: (Umbraco ? Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath : "") + "/api/FileLog/Get",
                logviewerurl: (Umbraco ? Umbraco.Sys.ServerVariables.umbracoSettings.appPluginsPath : "/App_Plugins") + "/LogViewer/logviewer.html"
            };
        });
        logViewer.controller("logViewer.AppController", [
            "$scope", "logViewer.urls", "assetsService", function (scope, urls, assetsService) {
                scope.logviewerurl = urls.logviewerurl;
                assetsService.loadCss(Umbraco.Sys.ServerVariables.umbracoSettings.appPluginsPath + "/LogViewer/logviewer.css");
            }
        ]);
        logViewer.controller("logViewer.Controller", ["logViewer.Service", "$scope", "$http", "logViewer.urls", LogViewerController]);
        logViewer.service("logViewer.Service", ["logViewer.urls", LogViewerService]);
        logViewer.directive("logFillscreen", [
            function() {
                function resize(element) {
                    var availHeight = $(window).height() - element.offset().top,
                        height = availHeight - 10;
                    element.height(height);
                }

                return function (scope, element, attrs) {
                    resize(element);
                    $(window).resize(function() { resize(element); });
                };
            }
        ]);
        logViewer.directive("logAutoscroll", [
            function () {
                function blink(element, orig, started, isStart) {
                    var time = new Date() - started,
                        isUp = time < 250,
                        newVal,
                        frame,
                        oriCol;

                    function rgb(r, g, b) {
                        return { r: r, g: g, b: b };
                    }

                    if (!isStart && time >= 500) {
                        element.css("background-color", orig);
                        return;
                    }
                    oriCol = eval(orig);
                    time = isUp ? time : time - 250;
                    frame = time / 250;
                    newVal = isUp ? {
                        r: parseInt(oriCol.r + frame * (255 - oriCol.r)),
                        g: parseInt(oriCol.g + frame * (255 - oriCol.g)),
                        b: parseInt(oriCol.b + frame * oriCol.b * -1)
                    } : {
                        r: parseInt(255 - frame * (255 - oriCol.r)),
                        g: parseInt(255 - frame * (255 - oriCol.g)),
                        b: parseInt(frame * (oriCol.b))
                    };
                    element.css("background-color", "rgb(" + newVal.r + ", " + newVal.g + ", " + newVal.b + ")");
                    setTimeout(function() {
                        blink(element, orig, started, false);
                    }, 1000/24);
                }

                return function(scope, element, attrs) {
                    var orig = element.css("background-color"),
                        autoscroll;

                    scope.$watch(function() {
                        return scope.$eval(attrs.ngModel).length;
                    }, function(newVal) {
                        if (autoscroll) {
                            element.scrollTop(element.prop("scrollHeight"));
                        } else if (newVal > 0) {
                            if (element.prop("scrollHeight") > element.innerHeight()) {
                                blink(element, orig, new Date(), true);
                            }
                        }
                    });

                    scope.$watch(function () {
                        return scope.$eval(attrs.logAutoscroll);
                    }, function(newValue) {
                        autoscroll = newValue;
                    });
                };
            }
        ]);
    }

    main();

}());