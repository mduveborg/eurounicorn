'use strict';

frontendApp.controller('SubmissionCtrl', function ($scope, $upload, $location) {
        var toUpload = [];
		$scope.submit = function (user) {
			$scope.message = "This unicorn is busy running your submission over to the unicorn base, so please be patient!";
			$upload.upload({
				url: '/api/submissions',
				method: 'POST',
				data: user,
				file: toUpload[0]
			}).progress(function (evt) {
			}).success(function(data, status, headers, config) {
				$location.path('/list');
			})
			.error( function(data){
				$scope.message = "Something unfortunate happened! And we are quite unsure what to do...";
			});
		};
        $scope.onFileSelect = function($files) {
			toUpload = $files;
        };
    });
