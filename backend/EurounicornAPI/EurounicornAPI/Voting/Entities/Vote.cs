using EurounicornAPI.CouchDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.Voting.Entities
{
    public class Vote : CouchObject
    {
        public string docType { get { return "vote"; } }

        public string Username { get; set; }
        public int Points { get; set; }
        public int TrackId { get; set; }
    }
}