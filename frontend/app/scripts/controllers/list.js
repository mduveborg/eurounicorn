'use strict';

angular.module('frontendApp')
	.controller('ListCtrl', function ($sce, $scope, $location, authService, $http) {
		$scope.playlist = "";
		$http({method: 'get',
			url: '/api/submissions/list'
		})
		.success(function (data) {
			$scope.playlist = $sce.trustAsHtml(data);
		})
		.error(function () {
			$location.path('/login');
		});
	});
