using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.DtoObjects
{
    public class VoteDto
    {
        public string Username { get; set; }
        public int Points { get; set; }
        public int TrackId { get; set; }
    }
}