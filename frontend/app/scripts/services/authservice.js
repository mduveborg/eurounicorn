'use strict';

angular.module('frontendApp')
	.factory('authService', ['$http', '$q', function ($http, $q) {
		var token;
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
						token = data.token;
						deferred.resolve(true);
					})
					.error(function (data) {
						deferred.reject(data);
					});
				return promise;

			}
		};
	}]);
