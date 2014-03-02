'use strict';

angular.module('frontendApp')
	.controller('NavCtrl', function ($sce, $scope, $location, authService, $http) {
		$scope.isActive = function (path) {
			return path === $location.path();
		};
		$scope.isAuthenticated = function () {
			return authService.isAuthenticated();
		};
	});
