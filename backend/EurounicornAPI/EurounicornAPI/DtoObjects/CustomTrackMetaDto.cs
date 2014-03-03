using EurounicornAPI.CouchDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.DtoObjects
{
    public class CustomTrackMetaDto : CouchObject
    {
        // Connection to sound cloud
        public int TrackId;
        public string docType { get { return "track"; } }
        
        // Custom meta information
        public string SongTitle;
        public string StageName;
        public string Musicians;
        public string Composers;
    }
}