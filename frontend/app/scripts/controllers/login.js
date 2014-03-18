'use strict';

angular.module('frontendApp')
	.controller('LoginCtrl', function ($scope, $location, authService) {
		$scope.submit = function () {
			var input = $scope.input;

			// If it is an email, check its an netlight email
			if(input.indexOf("@") != -1) {
				if(input.toLowerCase().indexOf("@netlight.") == -1) {
					$scope.message = "If using your email, please use your Netlight email";
					return;
				}
			}


			authService.login(input)
				.success(function (ok) {
					if (!ok) {
						return;
					}
					$scope.okMessage = "A unicorn is on its way with your super secret login link, keep a lookout!";
				})
				.error(function (data) {
					$scope.message = data;
				});
		};
	});
