'use strict';

var routes = {
	'/signin': {
		templateUrl: 'views/signin.html',
		controller: 'AuthCtrl'
	},
	'/': {
		templateUrl: 'views/main.html',
		controller: 'MainCtrl',
		requireLogin: true
	}
};

angular.module('frontendApp', [
  'ngCookies',
  'ngResource',
  'ngSanitize',
  'ngRoute',
  'Directives'
])
	.config(['$routeProvider', function ($routeProvider) {
		for (var path in routes) {
			$routeProvider.when(path, routes[path]);
		}
		$routeProvider.otherwise({ redirectTo: '/signin' });
	}])
	.run(function ($rootScope, $window, $location) {
		$rootScope.$on('$locationChangeStart', function (event) {
			if (true) {
				return;
			}
			if (routes[$location.path()] && routes[$location.path()].requireLogin && !$window.sessionStorage.token) {
				event.preventDefault();
				$location.path('/signin');
			}
		});
	});
