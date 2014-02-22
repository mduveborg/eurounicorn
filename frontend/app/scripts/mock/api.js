'use strict';

angular.module('frontendApp')
	.constant('Config', {
		useMocks:			true,
		viewDir:			'views/',
		API: {
			protocol:		'http',
			host:			'api.example.com',
			port:			'8080',
			path:			'/api',
			fakeDelay:		'250'
		}
	})
	.config(function (Config, $provide) {
		if (Config.useMocks) {
			$provide.decorator('$httpBackend', angular.mock.e2e.$httpBackendDecorator);
		}
	})
	.config(function ($httpProvider, Config) {
		if (!Config.useMocks) {
			return;
		}

		$httpProvider.interceptors.push(function ($q, $timeout, Config, $log) {
			return {
				request: function (config) {
					$log.log('Requesting ' + config.url, config);
					return config;
				},
				response: function (response) {
					var deferred = $q.defer();
					if (response.config.url.indexOf(Config.viewDir === 0)) {
						return response;
					}

					$log.log('Delaying response with ' + Config.API.fakeDelay + 'ms');
					$timeout(function () {
						deferred.resolve(response);
					}, Config.API.fakeDelay);
					return deferred.promise;
				}
			};
		});
	})
	.run(function (Config, $httpBackend) {
		if (!Config.useMocks) {
			return;
		}

		function regEsc(str) {
			return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\-\\\^\$\|]/g, '\\$&');
		}

		$httpBackend.whenGET( new RegExp( regEsc( Config.viewDir ) ) ).passThrough();

		$httpBackend.whenPOST('api/authenticate').respond(function (method, url, datat, headers) {
			return [200, { token: '123835jhndfjn' }, headers ];
		});

	});
