(function () {
    'use strict';
    angular.module('myApp', ['common.core', 'common.ui'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider', '$mdDateLocaleProvider', '$mdThemingProvider'];
    function config($routeProvider, $mdDateLocaleProvider, $mdThemingProvider) {
        $routeProvider
            .when("/", {
                templateUrl: 'scripts/app/account/login.html',
                controller: 'loginCtrl'
            })
            .when("/login", {
                templateUrl: 'scripts/app/account/login.html',
                controller: 'loginCtrl'
            })
            .when("/announcement", {
                templateUrl: 'scripts/app/announcement/annInfo.html',
                controller: 'annCtrl',
                resolve: { isAuthenticated: isAuthenticated }
            }).otherwise({ redirectTo: "/" });

        $mdDateLocaleProvider.formatDate = function (date) {
            return moment(date).format('DD-MM-YYYY');
        };
    }
    run.$inject = ['$rootScope', '$location', '$cookies', '$http', '$cookieStore'];
    function run($rootScope, $location, $cookies, $http, $cookieStore) {
        // handle page refreshes
        $rootScope.repository = $cookies.getObject('repository') || {};
        if ($rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authData;
        }
        console.log($rootScope.repository);
    }
    isAuthenticated.$inject = ['membershipService', '$rootScope', '$location'];
    function isAuthenticated(membershipService, $rootScope, $location) {
        if (!membershipService.isUserLoggedIn()) {
            $rootScope.previousState = $location.path();
            $location.path('/login');
        }
    }
})();