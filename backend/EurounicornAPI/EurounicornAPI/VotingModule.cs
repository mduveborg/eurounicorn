using System.Threading.Tasks;
using EurounicornAPI.CouchDB;
using EurounicornAPI.DtoObjects;
using EurounicornAPI.Voting;
using EurounicornAPI.Voting.Entities;
using Nancy;
using Nancy.ModelBinding;

namespace EurounicornAPI
{
    // Make a lazy instantiated CouchDBService
    public class VotingModule : NancyModule
    {
        public VotingModule() : base("api/vote")
        {
            var _couchDbService = new CouchDBService(); //make lazy

            this.Post["/", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var votesToPost = this.Bind<Vote>();
                    var userName = Context.CurrentUser.UserName;
                    var votingService = new VotingService(_couchDbService);

                    votingService.CastVote(userName, votesToPost.TrackId, votesToPost.Points);

                    return HttpStatusCode.OK;
                });
            };
            
            this.Get["/track", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var trackId = this.Bind<TrackDto>().Id;
                    var votingService = new VotingService(_couchDbService);

                    var votesForTrack = votingService.GetVotesForTrack(trackId);

                    return Response.AsJson(votesForTrack, HttpStatusCode.OK);
                });
            };

            this.Get["/turnout", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var votingService = new VotingService(_couchDbService);

                    var turnout = votingService.GetVoterTurnout();

                    return Response.AsJson(turnout, HttpStatusCode.OK);
                });
            };

            this.Get["/user", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var userName = Context.CurrentUser.UserName;
                    var votingService = new VotingService(_couchDbService);

                    var votesForUser = votingService.GetVotesForUser(userName);

                    return Response.AsJson(votesForUser, HttpStatusCode.OK);
                });
            };
        }
    }
}