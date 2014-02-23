using EurounicornAPI.SoundCloud;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI
{
    public class SubmissionModule : NancyModule
    {
        public SubmissionModule() : base("api/submission")
        {
            this.Post["/"] = _ =>
            {
                var file = this.Request.Files.FirstOrDefault();
                string title = this.Request.Query.Title;
                string filename = file.Name;
                var cloudService = new SoundCloudService();
                cloudService.Upload(new UploadTrack()
                {
                    Title = title,
                    Filename = filename,
                    Data = file.Value
                });
                return HttpStatusCode.OK;
            };
        }
    }
}