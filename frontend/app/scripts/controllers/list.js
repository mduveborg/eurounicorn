'use strict';

angular.module('frontendApp')
	.controller('ListCtrl', function ($sce, $scope, $location, authService, $http) {
		$scope.playlist = "Loading";
		$scope.trust = $sce.trustAsHtml;
		$scope.join = function (a, b, ch) {
			if (a && b) {
				return a + ' ' + ch + ' ' + b;
			}
			if (a) {
				return a;
			}
			if (!b) { 
				return '';
			}
			return b;
		};
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
