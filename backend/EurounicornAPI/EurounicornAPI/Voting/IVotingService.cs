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
        /// Gets the votes that the user with the supplied user ID has cast.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IEnumerable<Vote> GetVotesForUser(string username);

        /// <summary>
        /// Gets all votes for the track with the supplied track ID.
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        IEnumerable<Vote> GetVotesForTrack(int trackId);

        /// <summary>
        /// Casts a vote.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="trackId"></param>
        /// <param name="points"></param>
        void CastVote(string username, int trackId, int points);

        /// <summary>
        /// Gets the voting distribution with percentage per level.
        /// </summary>
        /// <returns></returns>
        IDictionary<Level, double> GetVotingDistribution();
    }
}