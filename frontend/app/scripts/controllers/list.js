'use strict';

frontendApp
    .controller('ListCtrl',
    [ '$sce', '$scope', '$location', 'authService', '$http', 'votingservice', '$q',
        function ($sce, $scope, $location, authService, $http, votingservice, $q) {

            $scope.playlist = "Loading";
            $scope.trust = $sce.trustAsHtml;

            $scope.points = [3, 2, 1];
            $scope.votes = {};

            $scope.submit = function() {
                var v = $scope.votes;
                if(v[1] && v[2] && v[3]) {

                    var votes = { vote1:{
                        Points: 1, TrackId: v[1]
                    },
                        vote2:{
                            Points: 2, TrackId: v[2]
                        },
                        vote3:{
                            Points: 3, TrackId: v[3]
                        }

                    };

                    $q.all([
                        votingservice.vote(votes.vote1),
                        votingservice.vote(votes.vote2),
                        votingservice.vote(votes.vote3)
                    ]).then(function (results) {
                        if(results[0]&&results[1]&&results[2]){
                            $scope.hasVoted = true;
                            alert( "Your votes are sent!");
                        }else{
                            alert("Ooops. A technical error occurred. Our developers are working hard to get the service up and running again!");
                        }
                    });

                } else {
                    alert("Please select 3 songs!")
                }
            };

            $scope.vote = function(point, trackId) {

                if($scope.hasVoted) {
                    alert("Your votes are already sent!");
                    return;
                }

                // If this point is already set, clear it
                for(var i = 0; i < $scope.points.length; i++) {
                    if($scope.votes[$scope.points[i]] == trackId) {
                        $scope.votes[$scope.points[i]] = null;
                    }
                }

                $scope.votes[point] = trackId;
            };

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

            //TODO: Move to votingservice
            $http({method: 'get',
                url: '/api/submissions/list'
            })
                .success(function (data) {
                    $scope.playlist = data;

                    $http({method: 'get', url: '/api/votes'})
                        .success(function (votes) {
                            $scope.votes = votes || {};
                            if(votes) {
                                $scope.hasVoted = true;
                            }

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

        }]);
