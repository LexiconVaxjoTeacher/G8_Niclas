angular.module('CommunityArenaApp.controllers', [])
.controller('forumController', function ($scope, $http) {
    $scope.name = "";
    $scope.subforums = {};
    $scope.threads = {};
    $scope.posts = {};
    $scope.mainForum = {};
    $scope.forumUsers = {};
    $scope.threadUsers = {};

    $scope.inputs = {};

    $scope.role = "";

    $scope.getRole = function () {
        $http({ url: "/Identity/GetRole", method: "GET", params: {} }).then(function (response) {
            $scope.role = response.data;
        });
    };
    
    $scope.getForumName = function (id) {
        $http({ url: "/Forum/GetForumName", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.name = response.data;
        });
    };

    $scope.getForumUsers = function (id) {
        $http({ url: "/Forum/GetForumUsers", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.forumUsers = response.data;
        });
    };

    $scope.getThreadUsers = function (id) {
        $http({ url: "/Forum/GetThreadUsers", method: "GET", params: { _threadId: id } }).then(function (response) {
            $scope.threadUsers = response.data;
        });
    };

    $scope.getForum = function (id) {
        $http({ url: "/Forum/GetForum", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.currentForum = response.data;

            $scope.getForumName($scope.currentForum.ParentForumID);
            $scope.getForumUsers(id);
            $scope.getSubForums(id);
            $scope.getThreads(id);
        });
    };

    $scope.getThread = function (id) {
        $http({ url: "/Forum/GetThread", method: "GET", params: { _threadId: id } }).then(function (response) {
            $scope.currentThread = response.data;

            $scope.getForumName($scope.currentThread.ForumID);
            $scope.getThreadUsers(id);
            $scope.getPosts(id);
        });
    }

    $scope.getSubForums = function (id) {
        $http({ url: "/Forum/GetSubForums", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.subforums = response.data;
        });
    };

    $scope.getThreads = function (id) {
        $http({ url: "/Forum/GetThreads", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.threads = response.data;
        });
    };

    $scope.updateValues = function (id) {
        $scope.getForumName(id);
        $scope.getSubForums(id);
        $scope.getThreads(id);
    };

    $scope.getPosts = function (id) {
        $http({ url: "/Forum/GetPosts", method: "GET", params: { _threadId: id } }).then(function (response) {
            $scope.posts = response.data;
            for (var i = 0; i < $scope.posts.length; i++) {
                $scope.posts[i].date = $scope.posts[i].PostTime;
                $scope.posts[i].date = new Date($scope.posts[i].PostTime.match(/\d+/)[0] * 1);
            }
        });
    };

    $scope.getMainForum = function () {
        $http({ url: "/Forum/GetMainForum", method: "GET", params: { } }).then(function (response) {
            $scope.mainForum = response.data;
            $scope.getForum($scope.mainForum.ID);
        });
    };

    $scope.getMainForum();
    $scope.getRole();

    $scope.makeForum = function (id) {
        console.log($scope.inputs.newForumName);
        $http({ url: "/Forum/CreateForum", method: "POST", params: { _name: $scope.inputs.newForumName, _parentForum: id } }).then(function () {
            $scope.getSubForums(id);
            $scope.inputs.newForumName = "";
        });
    };

    $scope.makeThread = function (id) {
        console.log($scope.inputs.newThreadName);
        $http({ url: "/Forum/CreateThread", method: "POST", params: { _name: $scope.inputs.newThreadName, _forum: id } }).then(function () {
            $scope.getThreads(id);
            $scope.inputs.newThreadName = "";
        });
    };

    $scope.makePost = function (id) {
        console.log($scope.inputs.newPostText);
        $http({ url: "/Identity/GetUser", method: "GET", params: {} }).then(function (response) {
            $http({ url: "/Forum/CreatePost", method: "POST", params: { _user: response.data, _thread: id, _text: $scope.inputs.newPostText } }).then(function () {
                $scope.getPosts(id);
                $scope.inputs.newPostText = "";
            });
        });
    };

    $scope.deleteForum = function (id) {
        $http({ url: "/Forum/DeleteForum", method: "POST", params: { _forum: id } }).then(function () {
            $scope.getSubForums($scope.currentForum.ID);
        });
    };

    $scope.deleteThread = function (id) {
        $http({ url: "/Forum/DeleteThread", method: "POST", params: { _thread: id } }).then(function () {
            $scope.getThreads($scope.currentForum.ID);
        });
    };

    $scope.deletePost = function (id) {
        $http({ url: "/Forum/DeletePost", method: "POST", params: { _post: id } }).then(function () {
            $scope.getPosts($scope.currentThread.ID);
        });
    };

    $scope.adminCheck = function () {
        if ($scope.role == "Admin") {
            return true;
        }
        return false;
    };

    $scope.userCheck = function () {
        if ($scope.role == "User" || $scope.role == "Admin") {
            return true;
        }
        return false;
    };


    $scope.test = function () {
        $http({ url: "/Identity/Test", method: "GET", params: {} }).then(function (response) {
            console.log(response.data);
        });
    };

    $scope.toForum = function (id) {
        $http({ url: "/Forum/UpdateForumPosition", method: "POST", params: { _forumId: id } }).then( function () {
            console.log("toForum " + id);
            $scope.getForum(id);
            $scope.currentTab = 'html/forum.html';
        });
    };

    $scope.toThread = function (id) {
        $http({ url: "/Forum/UpdateThreadPosition", method: "POST", params: { _threadId: id } }).then(function () {
            console.log("toThread " + id);
            $scope.getThread(id);
            $scope.currentTab = 'html/thread.html';
        });
    };

    $scope.tabs = [{
        title: 'Forum',
        url: 'html/forum.html'
    }, {
        title: 'Thread',
        url: 'html/thread.html'
    }];

    $scope.currentTab = 'html/forum.html';

    $scope.onClickTab = function (tab) {
        $scope.currentTab = tab.url;
    }

    $scope.isActiveTab = function (tabUrl) {
        return tabUrl == $scope.currentTab;
    }
});
