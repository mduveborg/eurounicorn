'use strict';

angular.module('frontendApp')
	.controller('MainCtrl', function ($scope, $http) {
		$scope.awesomeThings = [
			'HTML5 Boilerplate',
			'AngularJS',
			'Karma'
		];
        $scope.isActive = function (viewLocation) {
            return viewLocation === $location.path();
        };
	});
