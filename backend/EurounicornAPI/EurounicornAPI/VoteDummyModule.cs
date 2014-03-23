using EurounicornAPI.Voting;
using EurounicornAPI.Voting.Entities;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EurounicornAPI
{
    public class VoteDummyModule : AuthModule
    {
        public VoteDummyModule() : base("api/votingdummy")
        {
            this.Get["/putvote", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var username = "patric.ogren@netlight.com";
                    var level = Level.AC;
                    

                    var votingService = new VotingService(db);
                    votingService.AssertUser(username, level);


                    var trackId = 137943951;
                    var points = 2;



                    //votingService.CastVote(username, trackId, points);
                    //var votes = votingService.GetVotesForTrack(trackId);
                    var votes = votingService.GetVotesForUser(username);

                    //return Response.AsJson("vote is posted", HttpStatusCode.OK);
                    return Response.AsJson(votes, HttpStatusCode.OK);
                });
            };
        }
    }
}