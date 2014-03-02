using EurounicornAPI.SoundCloud;
using Nancy;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ewk.SoundCloud.ApiLibrary.Entities;
using AutoMapper;
using EurounicornAPI.AutoMapperResolvers;

namespace EurounicornAPI
{
    public class SubmissionModule : AuthModule
    {
        public SubmissionModule() : base("api/submissions")
        {
            this.Post["/", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var file = this.Request.Files.FirstOrDefault();
                    string filename = file.Name;
                    string title = this.Request.Form.Title;
                    if (title == null)
                        title = filename;
                    var cloudService = new SoundCloudService();
                    cloudService.Upload(new UploadTrack()
                    {
                        Title = title,
                        Filename = filename,
                        Data = file.Value,
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

                    // Convert SoundCloud Track into DTO object with relevant info
                    Mapper.CreateMap<Track, TrackDto>()
                        .ForMember(dest => dest.SoundCloudMeta, opt => opt.ResolveUsing<SoundCloudMetaResolver>());
                    var trackList = tracks.Result.ToList();
                    List<TrackDto> dtoList = new List<TrackDto>();
                    for (int i = 0; i < trackList.Count; i++)
                    {
                        dtoList.Add(Mapper.Map<TrackDto>(trackList[i]));
                    }

                    return Response.AsJson(dtoList, HttpStatusCode.OK);
                });
            };
        }
    }
}