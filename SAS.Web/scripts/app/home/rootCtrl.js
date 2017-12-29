(function (app) {
    'use strict';
    app.controller('rootCtrl', rootCtrl);
    rootCtrl.$inject = ['$scope', '$rootScope', '$location', 'membershipService'];
    function rootCtrl($scope, $rootScope, $location, membershipService) {
        $scope.logout = logout;
        function logout() {
            membershipService.removeCredentials();
            $location.path('/login');
        }
    }
})(angular.module('myApp'));