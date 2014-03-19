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
        private readonly ICouchDBService _couchDb;

        public VotingService(ICouchDBService couchDb)
        {
			_couchDb = couchDb;
        }

        public bool UserCanVote(string username)
        {
			var userVotes = GetVotesForUser(username);

			// Behöver vi explicit hantera fall där count == 1, 2 eller 3?
			return userVotes.Count(v => v.Username == username) == 0;
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
            var user = _couchDb.FindByUsername<User>(username).SingleOrDefault();

			if(user == null)
			{
				throw new InvalidOperationException("Could not find user " + username + ".");
			}

			if(UserCanVote(username))
			{
				var vote = new Vote { Username = user.Username, Points = points, TrackId = trackId };

				_couchDb.Set<Vote>(vote);
			}
        }

        public IDictionary<Level, double> GetVoterTurnout()
        {
			var turnouts = new Dictionary<Level, double>();

            foreach(Level level in Enum.GetValues(typeof(Level)))
            {
				var matchingUsers = _couchDb.FindUsersByLevel(level).ToList();

				// Check count to avoid division by zero
				if (matchingUsers.Count > 0)
				{
					var usersWithVotesCast = matchingUsers.Where(u => !UserCanVote(u.Username)).ToList();

					var voterCount = (double)usersWithVotesCast.Count;
					var userCount = (double)matchingUsers.Count;
					var turnout = Math.Round((voterCount / userCount) * 100, 1);

					turnouts.Add(level, turnout);
				}
            }

			return turnouts;
        }
    }
}