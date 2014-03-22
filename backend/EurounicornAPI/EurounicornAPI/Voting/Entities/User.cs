using EurounicornAPI.CouchDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.Voting.Entities
{
    public enum Level
    {
        Adm,    // Administration
        A,      // Analyst
        AC,     // Associate Consultant
        C,      // Consultant
        SrC,    // Senior Consultant
        SM,     // Solution Manager
        M,      // Manager
        SrM,    // Senior Manager
        P       // Partner

        // Summer intern?
        // Master thesis?
    }

    public class User : CouchObject
    {
        public string docType { get { return "user"; } }

        public string Username { get; set; }      // Email
        public Level Level { get; set; }

    }
}