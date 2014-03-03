'use strict';
var checkAuthResolver = ['$q', '$location', 'authService', function ($q, $location, auth) {
	var deferred = $q.defer();
	if ($location.search().token) {
		auth.setToken($location.search().token);
		$location.search('token', null);
	}
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
	'frontendApp.Services',
    'angularFileUpload'
])
	.config(function ($routeProvider) {
		$routeProvider
		.when('/', {
			templateUrl: 'views/main.html',
			controller: 'MainCtrl'
		})
		.when('/login', {
			templateUrl: 'views/login.html',
			controller: 'LoginCtrl'
		})
		.when('/logout', {
			templateUrl: 'views/login.html',
			controller: function ($location, auth) {
				auth.setToken(null);
				$location.path('/login');
			}
		})
		.when('/list', {
			templateUrl: 'views/list.html',
			controller: 'ListCtrl',
			resolve: {
				factory: checkAuthResolver
			}
		})
		.when('/submission', {
			templateUrl: 'views/submission.html',
			controller: 'SubmissionCtrl',
			resolve: {
				factory: checkAuthResolver
			}
		})
		.otherwise({
			redirectTo: '/'
		});
	});

