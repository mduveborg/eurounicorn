'use strict';

angular.module('frontendApp.Services', [])
	.factory('auth', [ '$window', function ($window) {
		return {
			setToken: function (token) {
				$window.localStorage.token = token;
			},
			isAuthenticated: function () {
				return !!$window.localStorage.token;
			},
			getToken: function () {
				return $window.localStorage.token;
			}
		};
	}])
	.factory('authInterceptor', ['$q', 'auth', '$location', function ($q, auth, $location) {
		return {
			request: function (config) {
				config.headers = config.headers || {};
				if (auth.isAuthenticated()) {
					config.headers.Authorization = auth.getToken();
				}
				return config;
			},
			response: function (response) {
				if (response.status === 401) {
					return $location.path('/login');
				}
				return response || $q.when(response);
			}
		};
	}])
	.config(['$httpProvider', function ($httpProvider) {
		$httpProvider.interceptors.push('authInterceptor');
	}])
	.factory('authService', ['$http', '$q', 'auth', function ($http, $q, auth) {
		return {
			login: function (email) {
				var deferred = $q.defer(),
					promise = deferred.promise;

				promise.success = function (fn) {
					promise.then(fn);
					return promise;
				};

				promise.error = function (fn) {
					promise.then(null, fn);
					return promise;
				};

				$http({method: 'POST',
					url: '/api/login',
					data: { email: email }
				})
					.success(function (data) {
						auth.setToken(data.token);
						deferred.resolve(true);
					})
					.error(function (data) {
						auth.setToken(null);
						deferred.reject(data);
					});
				return promise;
			},
			isAuthenticated: function () {
				return auth.isAuthenticated();
			},
			getToken: function () {
				return auth.getToken();
			}
		};
	}]);
