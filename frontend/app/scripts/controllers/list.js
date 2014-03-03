'use strict';

angular.module('frontendApp')
	.controller('ListCtrl', function ($sce, $scope, $location, authService, $http) {
		$scope.playlist = "";
		$scope.trust = $sce.trustAsHtml;
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
