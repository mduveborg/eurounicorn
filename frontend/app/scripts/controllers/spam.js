'use strict';

angular.module('frontendApp')
    .controller('SpamCtrl', function ($scope, $upload, $location) {
        var toUpload = [];
		$scope.submit = function (data) {
			$scope.message = "This unicorn is busy running your spam request over to the unicorn base, so please be patient!";
			$upload.upload({
				url: '/api/spam',
				method: 'POST',
				data: data,
				file: toUpload[0]
			}).progress(function (evt) {
			}).success(function(data, status, headers, config) {
				$scope.message = "Success, spam has been sent!";
			})
			.error( function(data){
				$scope.message = "Something unfortunate happened! And we are quite unsure what to do...";
			});
		};
        $scope.onFileSelect = function($files) {
			toUpload = $files;
        };
    });
