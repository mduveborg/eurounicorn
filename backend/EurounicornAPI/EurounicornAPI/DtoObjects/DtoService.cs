using AutoMapper;
using EurounicornAPI.Voting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.DtoObjects
{
    public class DtoService : IDtoService
    {
        public List<VoteDto> ConvertVotes(IEnumerable<Vote> votes)
        {
            // Convert SoundCloud Track into DTO object with relevant info
            Mapper.CreateMap<Vote, VoteDto>();
            //.ForMember(dest => dest.SoundCloudMeta, opt => opt.ResolveUsing<SoundCloudMetaResolver>());
            var voteList = votes.ToList();
            List<VoteDto> dtoList = new List<VoteDto>();
            foreach (var vote in voteList)
            {
                dtoList.Add(Mapper.Map<VoteDto>(vote));
            }

            return dtoList;
        }
    }
}