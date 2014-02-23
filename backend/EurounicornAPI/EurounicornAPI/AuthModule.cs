using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI
{
    public class AuthModule : NancyModule
    {
        public AuthModule()
        {
            Before += ctx =>
            {
                var user = FindUser(this.Request.Headers.Authorization);
                if (user != null)
                {
                    ctx.CurrentUser = user;
                    return null;
                }
                return HttpStatusCode.Unauthorized;
            };
        }

        private IUserIdentity FindUser(string authorization)
        {
            if (authorization == "dev")
                return new UserIdentity { UserName = "Unieurocorn" };
            return null;
        }
    }

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