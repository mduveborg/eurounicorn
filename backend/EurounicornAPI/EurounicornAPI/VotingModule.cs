using System.Threading.Tasks;
using EurounicornAPI.CouchDB;
using EurounicornAPI.DtoObjects;
using EurounicornAPI.Voting;
using EurounicornAPI.Voting.Entities;
using Nancy;
using Nancy.ModelBinding;

namespace EurounicornAPI
{
    public class VotingModule : NancyModule
    {
        private static IVotingService _votingService;

        public IVotingService VotingService
        {
            get { return _votingService ?? (_votingService = new VotingService(new CouchDBService())); }
        }

        public VotingModule() : base("api/vote")
        {
            this.Post["/", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var votesToPost = this.Bind<Vote>();
                    var userName = Context.CurrentUser.UserName;
                    var votingService = this.VotingService;

                    votingService.CastVote(userName, votesToPost.TrackId, votesToPost.Points);

                    return HttpStatusCode.OK;
                });
            };
            
            this.Get["/track", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var trackId = this.Bind<TrackDto>().Id;
                    var votingService = VotingService;

                    var votesForTrack = votingService.GetVotesForTrack(trackId);

                    return Response.AsJson(votesForTrack, HttpStatusCode.OK);
                });
            };

            this.Get["/turnout", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var votingService = VotingService;

                    var turnout = votingService.GetVoterTurnout();

                    return Response.AsJson(turnout, HttpStatusCode.OK);
                });
            };

            this.Get["/user", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var userName = Context.CurrentUser.UserName;
                    var votingService = VotingService;

                    var votesForUser = votingService.GetVotesForUser(userName);

                    return Response.AsJson(votesForUser, HttpStatusCode.OK);
                });
            };
        }
    }
}