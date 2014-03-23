using EurounicornAPI.Voting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurounicornAPI.DtoObjects
{
    public interface IDtoService
    {
        /// <summary>
        /// Indicates whether the user with the supplied user ID is allowed to vote.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        List<VoteDto> ConvertVotes(IEnumerable<Vote> votes);
    }
}
