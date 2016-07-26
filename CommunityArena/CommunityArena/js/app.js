/*Includes ng-route and our controller, simply makes sure that /mainPage.html is loaded into ng-view at start.
Also includes the forumController for the mainPage so that happens.*/
angular.module('CommunityArenaApp', [
    'CommunityArenaApp.controllers',
    /*'CommunityArenaApp.services',*/
    'ngRoute'
])
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when("/", {
        controller: "forumController",
        controllerAs: "vm",
        templateUrl: "/html/mainPage.html"
    });
}]);