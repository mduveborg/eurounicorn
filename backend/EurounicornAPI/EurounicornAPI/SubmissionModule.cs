using EurounicornAPI.SoundCloud;
using Nancy;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EurounicornAPI
{
    public class SubmissionModule : NancyModule
    {
        public SubmissionModule() : base("api/submission")
        {
            this.Post["/", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var file = this.Request.Files.FirstOrDefault();
                    string filename = file.Name;
                    string title = this.Request.Query.Title ?? filename;
                    var cloudService = new SoundCloudService();
                    cloudService.Upload(new UploadTrack()
                    {
                        Title = title,
                        Filename = filename,
                        Data = file.Value
                    });
                    return HttpStatusCode.OK;
                });
            };

            this.Get["/", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var cloudService = new SoundCloudService();
                    var token = cloudService.GetAccessToken();
                    var tracks = cloudService.GetTracksAsync(token);
                    tracks.Wait();
                    return Response.AsJson(tracks.Result.ToList(), HttpStatusCode.OK);
                });
            };
        }
    }
}