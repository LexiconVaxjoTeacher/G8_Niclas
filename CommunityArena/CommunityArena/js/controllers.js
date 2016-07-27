angular.module('CommunityArenaApp.controllers', [])
.controller('forumController', function ($scope, $http) {
    $scope.subforums = {};
    $scope.threads = {};

    $scope.getSubForums = function (id) {
        $http({ url: "/Forum/GetSubForums", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.subforums = response;
        });
    };

    $scope.getThreads = function (id) {
        $http({ url: "/Forum/GetThreads", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.threads = response;
        });
    };
});