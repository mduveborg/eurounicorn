using EurounicornAPI.CouchDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.Authentication
{

    public class TokenDto : CouchObject
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public string docType { get { return "token"; } }
    }
}