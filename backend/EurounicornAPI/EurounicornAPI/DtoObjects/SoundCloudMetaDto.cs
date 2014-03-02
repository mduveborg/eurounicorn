using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.DtoObjects
{
    /*
     * Underscore to facilitate AutoMapper mapping. Change to camel case before sending Json
     */
    public class SoundCloudMetaDto
    {
        public string State;
        public string StreamUrl;
        public string DownloadUrl;
        public int? PlaybackCount;
        public int Duration;
        public string CreatedAt;
        public string Downloadable;
    }
}