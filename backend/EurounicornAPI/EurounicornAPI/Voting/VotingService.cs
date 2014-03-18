using EurounicornAPI.CouchDB;
using EurounicornAPI.Voting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.Voting
{
    public class VotingService : IVotingService
    {
        private CouchDBService _couchDb;

        public VotingService()
        {
            _couchDb = new CouchDBService();
        }

        public bool UserCanVote(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vote> GetVotesForUser(string username)
        {
            return _couchDb.FindByUsername<Vote>(username);
        }

        public IEnumerable<Vote> GetVotesForTrack(int trackId)
        {
            return _couchDb.FindByTrackId<Vote>(trackId);
        }

        public void CastVote(string username, int trackId, int points)
        {
            var user = _couchDb.FindByUsername<User>(username).Single();

            var vote = new Vote { Username = user.Username, Points = points, TrackId = trackId };

            _couchDb.Set<Vote>(vote);
        }

        public IDictionary<Level, double> GetVotingDistribution()
        {
            // TODO
            //foreach(var level in Enum.GetNames(typeof(Level)))
            //{
            //    var matchingUsers = _couchDb.FindUserByLevel(level);

            //}
            return null;
        }
    }
}