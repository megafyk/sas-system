(function (app) {
    'use strict';

    app.controller('loginCtrl', loginCtrl);

    loginCtrl.$inject = ['$scope', 'membershipService', '$rootScope', '$location', '$interval'];
    function loginCtrl($scope, membershipService, $rootScope, $location, $interval) {

        $scope.pageClass = 'page-login';
        $scope.user = {
            username: '',
            password: '',
            rememberme: ''
        };
        $scope.maintMsg = {
            description: {
                rqdUsername: 'Username is required.',
                minUsername: 'Too short.',
                maxUsername: 'Too long.',
                rqdPassword: 'Password is required.',
                dirtyPassword: 'Wrong password.'
            }
        };

        $scope.clear = function () {
            $scope.user = {
                username: '',
                password: '',
                rememberme: ''
            };
            $scope.msgLogin = '';
            $scope.Form.$setUntouched();
            $scope.Form.$setPristine();
            document.getElementById('inputUsr').focus();
        };

        $scope.login = login;
        function login() {
            $scope.msgLogin = '';
            membershipService.login($scope.user, loginCompleted);
        };

        function loginCompleted(result) {
            if (result.data.success) {
                var today = new Date();
                var expires = new Date(today);
                console.log(today);
                if (!$scope.user.rememberme) {
                    console.log(today.getUTCMinutes());
                    expires.setUTCMinutes(today.getUTCMinutes() + 30);
                    console.log(expires);
                } else {
                    expires.setUTCMinutes(today.getUTCMinutes() + 1440);
                    console.log(expires);
                }
                membershipService.saveCredentials($scope.user, expires);
                if ($rootScope.previousState) {
                    $location.path($rootScope.previousState);
                } else {
                    $location.path('/announcement');
                }
                console.log($rootScope.repository);
            } else {
                $scope.msgLogin = result.data.serverMsg;
            }
        };
    }
})(angular.module('common.core'));