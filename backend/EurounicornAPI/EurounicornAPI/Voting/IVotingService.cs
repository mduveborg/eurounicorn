using EurounicornAPI.DtoObjects;
using EurounicornAPI.Voting.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.Voting
{
    public interface IVotingService
    {
        /// <summary>
        /// Indicates whether the user with the supplied user ID is allowed to vote.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool UserCanVote(string username);

        /// <summary>
        /// Indicates whether the user with the supplied user ID is allowed to vote for
        /// the track with the supplied trackId.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="trackId"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        bool UserCanVote(string username, int trackId, int points);

        /// <summary>
        /// Gets the votes that the user with the supplied user ID has cast.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IEnumerable<VoteDto> GetVotesForUser(string username);

        /// <summary>
        /// Gets all votes for the track with the supplied track ID.
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        IEnumerable<VoteDto> GetVotesForTrack(int trackId);

        /// <summary>
        /// Casts a vote.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="trackId"></param>
        /// <param name="points"></param>
        void CastVote(string username, int trackId, int points);

        /// <summary>
        /// Gets the voter turnout per level.
        /// </summary>
        /// <returns></returns>
        IDictionary<Level, double> GetVoterTurnout();

        /// <summary>
        /// Makes sure that the User object existst and has a Level.
        /// </summary>
        /// <returns></returns>
        void AssertUser(string username, Level level);
    }
}