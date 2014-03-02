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
			templateUrl: 'views/list.html',
			controller: 'ListCtrl'
		})
            .when('/submission', {
                templateUrl: 'views/submission.html',
                controller: 'SubmissionCtrl'
            })
		.otherwise({
			redirectTo: '/'
		});
	});

