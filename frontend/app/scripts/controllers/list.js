'use strict';

angular.module('frontendApp')
	.controller('ListCtrl', function ($scope, $location, authService) {
		$http({method: 'get',
			url: '/api/submissions/list'
		})
		.success(function (data) {
			$scope.playlist = data;
		})
		.error(function () {
			$location.path('/login');
		});
	});
