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

    //{"kind":"track",
    //"id":137616160,
    //"created_at":"2014/03/03 01:02:24 +0000",
    //"user_id":81598671,"duration":0,"commentable":true,"state":"processing","original_content_size":null,"sharing":"private","tag_list":"","permalink":"213123-1","streamable":true,"embeddable_by":"all","downloadable":false,"purchase_url":null,"label_id":null,"purchase_title":null,"genre":null,"title":"213123","description":null,"label_name":null,"release":null,"track_type":null,"key_signature":null,"isrc":null,"video_url":null,"bpm":null,"release_year":null,"release_month":null,"release_day":null,"original_format":"unknown","license":"all-rights-reserved","uri":"https://api.soundcloud.com/tracks/137616160","user":{"id":81598671,"kind":"user","permalink":"eurounicorn","username":"eurounicorn","uri":"https://api.soundcloud.com/users/81598671","permalink_url":"http://soundcloud.com/eurounicorn","avatar_url":"https://a1.sndcdn.com/images/default_avatar_large.png?435a760"},"shared_to_count":0,"user_playback_count":1,"user_favorite":false,"permalink_url":"http://soundcloud.com/eurounicorn/213123-1","artwork_url":null,"waveform_url":"https://a1.sndcdn.com/images/player-waveform-medium.png?435a760","stream_url":"https://api.soundcloud.com/tracks/137616160/stream","download_url":"https://api.soundcloud.com/tracks/137616160/download","playback_count":0,"download_count":0,"favoritings_count":0,"comment_count":0,"attachments_uri":"https://api.soundcloud.com/tracks/137616160/attachments","secret_token":"s-TH2OS","secret_uri":"https://api.soundcloud.com/tracks/137616160?secret_token=s-TH2OS","downloads_remaining":100}

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
                    var response = cloudService.Upload(new UploadTrack()
                    {
                        Title = title,
                        Filename = filename,
                        Data = file.Value,
                    });

                    // Get the track id of the uploaded track
                    int trackId = response.id;

                    // Store custom meta information to database.
                    if (trackId > 0)
                    {
                        CustomTrackMetaDto dto = new CustomTrackMetaDto();
                        dto.TrackId = trackId;

                        // Add the meta information
                        dto.SongTitle = this.Request.Form.SongTitle;
                        dto.StageName = this.Request.Form.StageName;
                        dto.Musicians = this.Request.Form.Musicians;
                        dto.Composers = this.Request.Form.Composers;
                        
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

            this.Get["/list", true] = (_, cancel) =>
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
                    foreach (var track in trackList)
                    {
                        var tra = Mapper.Map<TrackDto>(track);
                        tra.Embed = Embedd(track.id, track.secret_token);
                        dtoList.Add(tra);
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
        }

        private string Embedd(int trackId, string secret)
        {
            return @"<iframe width=""100%"" height=""300"" scrolling=""no"" frameborder=""no"" src=""https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/tracks/" + 
                trackId + "%3Fsecret_token%3D" + secret + @"&amp;auto_play=false&amp;hide_related=false&amp;visual=true""></iframe>";
        }
    }
}