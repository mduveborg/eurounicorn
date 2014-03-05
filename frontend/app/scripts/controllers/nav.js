'use strict';

angular.module('frontendApp')
    .controller('NavCtrl', function ($scope, $location, authService) {
        $scope.isActive = function (viewLocation) {
            return viewLocation === $location.path();
        };
        $scope.isAuthenticated = authService.isAuthenticated;
    });
