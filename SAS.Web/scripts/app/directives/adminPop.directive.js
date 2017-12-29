(function (app) {
    'use strict';

    app.directive('modal', modal);

    function modal() {
        return {
            scope: false,
            restrict: 'E',
            replace: true,
            link: function (scope, element, attrs) {
                scope.getContentUrl = function () {
                    if (attrs.user == '01')
                        return 'scripts/app/directives/adminPop.html';
                    return 'scripts/app/directives/staffPop.html'
                };
            },
            template: '<div ng-include="getContentUrl()"></div>'
        }
    }

})(angular.module('myApp'));