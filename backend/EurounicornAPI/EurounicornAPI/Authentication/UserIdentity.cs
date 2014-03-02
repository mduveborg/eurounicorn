using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.Authentication
{

    public class UserIdentity : IUserIdentity
    {

        public IEnumerable<string> Claims
        {
            get { yield break; }
        }

        public string UserName
        {
            get;
            set;
        }
    }
}