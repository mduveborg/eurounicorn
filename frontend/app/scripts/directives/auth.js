'use strict';

angular.module('Directives', [])
	.directive('euAuth', function () {
		return {
			restrict: 'A',
			templateUrl: 'views/auth.html'
		};
	});
