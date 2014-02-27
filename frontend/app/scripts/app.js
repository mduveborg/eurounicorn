'use strict';
var checkAuthResolver = ['$q', '$location', 'authService', function ($q, $location, auth) {
	var deferred = $q.defer();
	if (auth.isAuthenticated()) {
		deferred.resolve(true);
	} else {
		deferred.reject();
		$location.path('/login');
	}
	return deferred.promise;
}];

angular.module('frontendApp', [
	'ngCookies',
	'ngResource',
	'ngSanitize',
	'ngRoute',
	'frontendApp.Services'
])
	.config(function ($routeProvider) {
		$routeProvider
		.when('/', {
			templateUrl: 'views/main.html',
			controller: 'MainCtrl',
			resolve: {
				factory: checkAuthResolver
			}
		})
		.when('/login', {
			templateUrl: 'views/login.html',
			controller: 'LoginCtrl'
		})
		.when('/list', {
			templateUrl: 'views/list.html'
		})
		.otherwise({
			redirectTo: '/'
		});
	});

