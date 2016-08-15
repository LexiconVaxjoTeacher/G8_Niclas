angular.module('CommunityArenaApp.controllers', [])
.controller('forumController', function ($scope, $http) {
    $scope.name = "";
    $scope.subforums = {};
    $scope.threads = {};
    $scope.posts = {};
    $scope.mainForum = {};
    $scope.forumUsers = {};
    $scope.threadUsers = {};
    $scope.threadTarget = "";

    $scope.fighterShown = {};
    $scope.showingFighter = false;

    $scope.inputs = {};

    $scope.currentUsername = "";

    $scope.role = "";

    $scope.showStats = function (username) {
        $http({ url: "/Fighter/GetFighter", method: "GET", params: { _username: username } }).then(function (response) {
            $scope.fighterShown = response.data;
            $scope.showingFighter = true;
        });
    }

    // This function retrieves the role of the current user, so that I can use it for authorization in Angular.
    $scope.getRole = function () {
        $http({ url: "/Identity/GetRole", method: "GET", params: {} }).then(function (response) {
            $scope.role = response.data;
        });
    };

    $scope.getUserName = function () {
        $http({ url: "/Identity/GetUser", method: "GET", params: {} }).then(function (response) {
            $scope.currentUsername = response.data;
        });
    };
    
    // This function gets the name of a forum and stores it in name.
    $scope.getForumName = function (id) {
        $http({ url: "/Forum/GetForumName", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.name = response.data;
        });
    };

    // This function gets the users that are currently browsing the current forum and stores them in forumUsers.
    $scope.getForumUsers = function (id) {
        $http({ url: "/Forum/GetForumUsers", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.forumUsers = response.data;
        });
    };

    // This function gets the users that are currently browsing the current thread and stores them in threadUsers.
    $scope.getThreadUsers = function (id) {
        $http({ url: "/Forum/GetThreadUsers", method: "GET", params: { _threadId: id } }).then(function (response) {
            $scope.threadUsers = response.data;
        });
    };

    // This function gets the forum we're going to and throws out all functions needed to correctly transition to the new forum.
    $scope.getForum = function (id) {
        $http({ url: "/Forum/GetForum", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.currentForum = response.data;

            $scope.getForumName($scope.currentForum.ParentForumID);
            $scope.getForumUsers(id);
            $scope.getSubForums(id);
            $scope.getThreads(id);
        });
    };

    // This function of the thread we're currently moving to and goes through all the functions for proper transitioning.
    $scope.getThread = function (id) {
        $http({ url: "/Forum/GetThread", method: "GET", params: { _threadId: id } }).then(function (response) {
            $scope.currentThread = response.data;

            $scope.getForumName($scope.currentThread.ForumID);
            $scope.getThreadUsers(id);
            $scope.getPosts(id);
        });
    }

    // Gets all subforums linked to the current forum, so that we can travel to them.
    $scope.getSubForums = function (id) {
        $http({ url: "/Forum/GetSubForums", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.subforums = response.data;
        });
    };

    // Gets all threads which exists in the current forum, so that we can travel to them.
    $scope.getThreads = function (id) {
        $http({ url: "/Forum/GetThreads", method: "GET", params: { _forumId: id } }).then(function (response) {
            $scope.threads = response.data;
            /*for (var i = 0; i < $scope.threads.length; i++) {
                $scope.threads[i].watchers = "";
            }*/
            $scope.getThreadWatchers();
        });
    };

    $scope.threadIds = []
    $scope.watchers = []
    $scope.getThreadWatchers = function () {
        $scope.threadIds = []
        for (var i = 0; i < $scope.threads.length; i++) {
            $scope.threadIds.push($scope.threads[i].ID)
        }
        $http({ url: "/Forum/GetThreadUsers", method: "POST", params: { _threadIds: $scope.threadIds } }).then(function (response) {
            console.log(response.data);
            for (var i = 0; i < response.data.length; i++) {
                $scope.watchers.push(response.data[i]);
            }
        });
    };

    // ... Test function, I think I can remove it, but I'm afraid to do so.
    $scope.updateValues = function (id) {
        $scope.getForumName(id);
        $scope.getSubForums(id);
        $scope.getThreads(id);
    };

    // Gets all the posts that are in the thread we're heading towards.
    // Due to date being one of the variables in a post, we need to make a transition from C# DateTime to Javascript Date.
    $scope.getPosts = function (id) {
        $http({ url: "/Forum/GetPosts", method: "GET", params: { _threadId: id } }).then(function (response) {
            $scope.posts = response.data;
            for (var i = 0; i < $scope.posts.length; i++) {
                $scope.posts[i].date = $scope.posts[i].PostTime;
                $scope.posts[i].date = new Date($scope.posts[i].PostTime.match(/\d+/)[0] * 1);
            }
        });
    };

    // Gets the ID of the main forum, and then makes us go to it. The first function to be called.
    $scope.getMainForum = function () {
        $http({ url: "/Forum/GetMainForum", method: "GET", params: { } }).then(function (response) {
            $scope.mainForum = response.data;
            $scope.getForum($scope.mainForum.ID);
        });
    };

    $scope.getMainForum();
    $scope.getRole();
    $scope.getUserName();

    // Function called when creating a forum. Takes a parameter that is the current forum which the new forum will be a subforum to.
    // Also retrieves newForumTitle, which is written in from the html.
    $scope.makeForum = function (id) {
        console.log($scope.inputs.newForumName);
        $http({ url: "/Forum/CreateForum", method: "POST", params: { _name: $scope.inputs.newForumName, _parentForum: id } }).then(function () {
            $scope.getSubForums(id);
            $scope.inputs.newForumName = "";
        });
    };

    // Function called when creating a thread. Takes a parameter that is the current forum which will contain the thread.
    // Also retrieves newThreadTitle, which is written in from the html.
    $scope.makeThread = function (id) {
        console.log($scope.inputs.newThreadName);
        $http({ url: "/Forum/CreateThread", method: "POST", params: { _name: $scope.inputs.newThreadName, _forum: id } }).then(function () {
            $scope.getThreads(id);
            $scope.inputs.newThreadName = "";
        });
    };

    // Function called when creating a post. Takes a parameter that is the current thread, which the post will be posted in.
    // It sends along currentUsername, which is the username of the current user. newPostText tells what the post will contain, it is written in the html.
    $scope.makePost = function (id) {
        console.log($scope.inputs.newPostText);
            $http({ url: "/Forum/CreatePost", method: "POST", params: { _user: $scope.currentUsername, _thread: id, _text: $scope.inputs.newPostText } }).then(function () {
                $scope.getPosts(id);
                $scope.inputs.newPostText = "";
            });
    };

    // Function called when deleting a forum. Calls new subforums to the current forum to update the subforum-list after.
    $scope.deleteForum = function (id) {
        $http({ url: "/Forum/DeleteForum", method: "POST", params: { _forum: id } }).then(function () {
            $scope.getSubForums($scope.currentForum.ID);
        });
    };

    // Function called when deleting a thread. Calls new threads to the current forum to update the thread-list after.
    $scope.deleteThread = function (id) {
        $http({ url: "/Forum/DeleteThread", method: "POST", params: { _thread: id } }).then(function () {
            $scope.getThreads($scope.currentForum.ID);
        });
    };

    // Function called when deleting a Post. Calls new posts to the current thread to update the post-list after.
    $scope.deletePost = function (id) {
        $http({ url: "/Forum/DeletePost", method: "POST", params: { _post: id } }).then(function () {
            $scope.getPosts($scope.currentThread.ID);
        });
    };

    // Checks if the user is admin. Done for authorization purposes.
    $scope.adminCheck = function () {
        if ($scope.role == "Admin") {
            return true;
        }
        return false;
    };

    // Checks if person is a user. Done for authorization purposes.
    $scope.userCheck = function () {
        if ($scope.role == "User" || $scope.role == "Admin") {
            return true;
        }
        return false;
    };

    $scope.notCurrentUserCheck = function (username) {
        if ($scope.role == "User" || $scope.role == "Admin") {
            if ($scope.currentUsername != username) {
                return true;
            }
        }
        return false;
    }

    $scope.targetCheck = function () {
        if ($scope.threadTarget != "")
        {
            return true;
        }
        return false;
    };

    $scope.test = function () {
        $http({ url: "/Identity/Test", method: "GET", params: {} }).then(function (response) {
            console.log(response.data);
        });
    };

    // Each time the user moves among forums, this function will update their position in the database.
    $scope.toForum = function (id) {
        $http({ url: "/Forum/UpdateForumPosition", method: "POST", params: { _forumId: id } }).then( function () {
            console.log("toForum " + id);
            $scope.getForum(id);
            $scope.currentTab = 'html/forum.html';
        });
    };

    // Each time the user moves among threads, this function will update their position in the database.
    $scope.toThread = function (id) {
        $http({ url: "/Forum/UpdateThreadPosition", method: "POST", params: { _threadId: id } }).then(function () {
            console.log("toThread " + id);
            $scope.getThread(id);
            $scope.currentTab = 'html/thread.html';
        });
    };

    $scope.openInteraction = function (username) {
        $http({ url: "/Forum/OpenInteraction", method: "POST", params: { _targetUser: username, _currentForum: $scope.currentForum.ID } }).then(function (thread) {
            $http({ url: "/Forum/GetThreadTarget", method: "GET", params: { _threadId: thread.data } }).then(function (targetName) {
                $scope.threadTarget = targetName.data;
                $scope.toThread(thread.data);
            });
        });
    };

    $scope.attack = function (username) {
        $http({ url: "/Forum/Attack", method: "POST", params: { _targetUser: username, _currentForum: $scope.currentForum.ID, _currentThread: $scope.currentThread.ID } }).then(function (thread) {
            $scope.getPosts($scope.currentThread.ID);
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
