using AutoMapper;
using EurounicornAPI.CouchDB;
using EurounicornAPI.DtoObjects;
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
        public IDtoService _dtoService;

        public VotingService(ICouchDBService couchDb)
        {
			_couchDb = couchDb;
            _dtoService = new DtoService();
        }

		public bool UserCanVote(string username)
		{
			return UserCanVote(username, 0);
		}

        public bool UserCanVote(string username, int trackId)
        {
			var userVotes = GetVotesForUser(username);

			var isTrackVoteable = true;

			if(trackId > 0)
			{
				isTrackVoteable = !userVotes.Any(v => v.TrackId == trackId);
			}

			return isTrackVoteable && userVotes.Count(v => v.Username == username) < 3;
        }

        public IEnumerable<VoteDto> GetVotesForUser(string username)
        {
            var votes = _couchDb.FindByUsername<Vote>(username);
            return _dtoService.ConvertVotes(votes);
        }

        public IEnumerable<VoteDto> GetVotesForTrack(int trackId)
        {
            var votes = _couchDb.FindByTrackId<Vote>(trackId);
            return _dtoService.ConvertVotes(votes);
        }

		public void CastVote(string username, int trackId, int points)
		{
			var user = _couchDb.FindByUsername<User>(username).SingleOrDefault();

			if (user == null)
			{
				throw new InvalidOperationException("Could not find user " + username + ".");
			}

			if (UserCanVote(username, trackId))
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
				var matchingUsers = _couchDb.FindByLevel<User>(level).ToList();

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

        public void AssertUser(string username, Level level)
        {
            var user = _couchDb.FindByUsername<User>(username).SingleOrDefault();

            if (user == null)
            {
                user = new User();
                user.Username = username;
                user.Level = level;
                _couchDb.Set<User>(user);
            }
            else if (user.Level != level)
            {
                throw new InvalidOperationException("The asserted level " + level + " does not match for user " + username + ".");
            }
        }
    }
}