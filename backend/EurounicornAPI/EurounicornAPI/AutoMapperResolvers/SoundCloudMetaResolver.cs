using AutoMapper;
using EurounicornAPI.DtoObjects;
using Ewk.SoundCloud.ApiLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.AutoMapperResolvers
{

    public class SoundCloudMetaResolver : ValueResolver<Track, SoundCloudMetaDto>
    {
        protected override SoundCloudMetaDto ResolveCore(Track track)
        {
            Mapper.CreateMap<Track, SoundCloudMetaDto>();
            SoundCloudMetaDto dto = Mapper.Map<SoundCloudMetaDto>(track);
            
            // Other non-trivial mappings
            dto.DownloadUrl = track.download_url;
            dto.StreamUrl = track.stream_url;
            dto.PlaybackCount = track.playback_count;
            dto.CreatedAt = track.created_at;
            
            return dto;
        }
    }
}