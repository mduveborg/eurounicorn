'use strict';

angular.module('frontendApp')
	.controller('LoginCtrl', function ($scope, $location, authService) {
		$scope.submit = function () {
			authService.login('daniel.werthen@netlight.com')
				.success(function (ok) {
					if (!ok) {
						return;
					}
					$location.path('/');
				})
				.error(function (data) {
					$scope.message = data;
				});
		};
		$scope.awesomeThings = [
			'HTML5 Boilerplate',
			'AngularJS',
			'Karma'
		];
	});
