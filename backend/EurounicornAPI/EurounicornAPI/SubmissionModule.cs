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
using EurounicornAPI.DtoObjects;
using EurounicornAPI.CouchDB;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

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
                    string response = cloudService.Upload(new UploadTrack()
                    {
                        Title = title,
                        Filename = filename,
                        Data = file.Value,
                    });

                    // Get the track id of the uploaded track
                    int trackId = -1;
                    Match match = Regex.Match(response, "^.*\"id\":([0-9]*);.*$");
                    if (match.Success) trackId = Convert.ToInt32(match.Groups[1].Value);

                    // Store custom meta information to database.
                    if (trackId > 0)
                    {
                        CustomTrackMetaDto dto = new CustomTrackMetaDto();
                        dto.TrackId = trackId;

                        // Add the meta information
                        dto.Author = "TestAuthor2";

                        var db = new CouchDBService();
                        db.Set<CustomTrackMetaDto>(dto);
                    }

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

                    // Get other track information stored in CouchDB
                    var db = new CouchDBService();
                    for (int i = 0; i < dtoList.Count; i++)
                    {
                        TrackDto dto = dtoList[i];
                        
                        // Add custom meta information
                        CustomTrackMetaDto meta = null;
                        CustomTrackMetaDto[] metaObjects = db.FindByTrackId<CustomTrackMetaDto>(dto.Id).ToArray();
                        if (metaObjects.Length > 0) meta = metaObjects[0];
                        dto.CustomTrackMeta = meta;
                    }

                    return Response.AsJson(dtoList, HttpStatusCode.OK);
                });
            };

            this.Get["/list"] = ctx =>
            {
                return @"<iframe width=""100%"" height=""450"" scrolling=""no"" frameborder=""no"" src=""https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/playlists/24937343%3Fsecret_token%3Ds-3JmvU&amp;color=ff5500&amp;auto_play=false&amp;hide_related=false&amp;show_artwork=true""></iframe>";
            };
        }
    }
}