'use strict';

frontendApp.factory('votingservice',
    [ '$http', '$q',
    function ($http, $q) {
        return {
            vote: function (vote) {
                var deferred = $q.defer(),
                    promise = deferred.promise;

                $http({method: 'POST', url: '/api/votes', data: vote}).
                    success(function (data, status, headers, config) {
                        //TODO: implement data, status.
                        deferred.resolve(true);
                    }).
                    error(function (data, status, headers, config) {
                        //TODO: implement data, status.
                        deferred.reject(false);
                    });
                return promise;
            }
        }}]);