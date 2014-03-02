using AutoMapper;
using EurounicornAPI.Beans;
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
            return Mapper.Map<SoundCloudMetaDto>(track);
        }
    }
}