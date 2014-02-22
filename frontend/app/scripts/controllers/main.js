'use strict';

angular.module('frontendApp')
	.controller('MainCtrl', function ($scope) {
		$scope.submissions = [
			{ name: 'Kalles song' },
			{ name: 'Annas song' }
		];

		$scope.awesomeThings = [
			'HTML5 Boilerplate',
			'AngularJS',
			'Karma'
		];
	})
	.controller('AuthCtrl', function ($scope, $http, $window) {
		$scope.user = { email: '' };
		$scope.submit = function () {
			$http
				.post('api/authenticate', $scope.user)
				.success(function (data) {
					$window.sessionStorage.token = data.token;
					$scope.message = 'Welcome';
				})
				.error(function () {
					delete $window.sessionStorage.token;
					$scope.message = 'Error: Invalid email';
				});
		};
	})
	.factory('authInterceptor', function ($rootScope, $q, $window) {
		return {
			request: function (config) {
				config.headers = config.headers || {};
				if ($window.sessionStorage.token) {
					config.headers.Authorization = 'Bearer ' + $window.sessionStorage.token;
				}
				return config;
			},
			response: function (response) {
				if (response.status === 401) {
					//Not authenticated
				}
				return response || $q.when(response);
			}
		};
	})
	.config(function ($httpProvider) {
		$httpProvider.interceptors.push('authInterceptor');
	});
