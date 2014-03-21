'use strict';

angular.module('frontendApp')
	.controller('ListCtrl', function ($sce, $scope, $location, authService, $http) {
		$scope.playlist = "Loading";
		$scope.trust = $sce.trustAsHtml;

		$scope.points = [3, 2, 1];
		$scope.votes = {};

		$scope.submit = function() {
			var v = $scope.votes;
			if(v[1] && v[2] && v[3]) {

				$http({method: 'POST', url: '/api/votes', data: v}).
				    success(function(data, status, headers, config) {
				    	alert("Your votes are sent!")
				    }).
				    error(function(data, status, headers, config) {
				      	alert("Ooops. A technical error occured. Our developers are working hard to get the service up and running again!")
				    });

			} else {
				alert("Please select 3 songs!")
			}			
		}

		$scope.vote = function(point, trackId) {
			// If this point is already set, clear it
			for(var i = 0; i < $scope.points.length; i++) {
				if($scope.votes[$scope.points[i]] == trackId) {
					$scope.votes[$scope.points[i]] = null;
				}
			}

			$scope.votes[point] = trackId;
		}	


		$scope.join = function (a, b, ch) {
			if (a == b) {
				return a;
			}
			if (a && b) {
				return a + ' ' + ch + ' ' + b;
			}
			if (a) {
				return a;
			}
			if (!b) { 
				return '';
			}
			return b;
		};

		$http({method: 'get',
			url: '/api/submissions/list'
		})
		.success(function (data) {
			$scope.playlist = data;

			$http({method: 'get', url: '/api/votes'})
				.success(function (votes) {
					$scope.votes = votes;
				})
				.error(function () {
					$location.path('/login');
				});

		})
		.error(function () {
			$location.path('/login');
		});


		$scope.findTrack = function(id) {
			for(var i = 0; i < $scope.playlist.length; i++) {
				var track = $scope.playlist[i];
				if(track.id == id) {
					return track;
				}
			}
		}

	});
