'use strict';

angular.module('frontendApp')
	.controller('MainCtrl', function ($scope, $http) {
		$http({ method: 'GET', url: '/api/submissions' })
			.success(function (data) {
				console.dir(data);
			})
			.error(function (data) {
				console.dir(data);
				$scope.error = data;
			});
		$scope.awesomeThings = [
			'HTML5 Boilerplate',
			'AngularJS',
			'Karma'
		];
	});
