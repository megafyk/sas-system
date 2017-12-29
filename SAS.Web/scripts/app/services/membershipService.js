(function (app) {
    'use strict';
    app.factory('membershipService', membershipService);

    membershipService.$inject = ['apiService', '$http', '$rootScope', '$base64', '$cookies', '$cookieStore'];

    function membershipService(apiService, $http, $rootScope, $base64, $cookies, $cookieStore) {
        var service = {
            login: login,
            saveCredentials: saveCredentials,
            removeCredentials: removeCredentials,
            isUserLoggedIn: isUserLoggedIn,

        }
        function login(user, completed) {
            apiService.post('/api/account/authenticate', user, completed, loginFailed);
        }
        function loginFailed(response) {
            console.log('Failed to connect to server');
        }
        function saveCredentials(user, expires) {
            var membershipData = $base64.encode(user.username + ':' + user.password);
            $rootScope.repository = {
                loggedUser: {
                    username: user.username,
                    authData: membershipData,
                    expires: expires
                }
            };
            $http.defaults.headers.common['Authorization'] = 'Basic ' + membershipData;
            $cookies.putObject('repository', $rootScope.repository, { expires: expires });
        }
        function removeCredentials() {
            $rootScope.repository = {};
            $cookies.remove('repository');
            $http.defaults.headers.common.Authorization = '';
        }
        function isUserLoggedIn() {
            return $rootScope.repository.loggedUser != null;
        }
        return service;
    }
})(angular.module('common.core'));