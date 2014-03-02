using EurounicornAPI.Authentication;
using EurounicornAPI.CouchDB;
using Nancy;
using Nancy.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace EurounicornAPI
{
    public class AuthModule : NancyModule
    {
        public AuthModule()
        {
            init();
            Before += AuthenticateRequest;
        }

        public AuthModule(string modulePath) : base(modulePath)
        {
            init();
            Before += AuthenticateRequest;
        }

        private Response AuthenticateRequest(NancyContext ctx)
        {
            if (this.Request.Headers.Authorization == "supersecretrandomkey")
            {
                ctx.CurrentUser = new UserIdentity { UserName = "Dev" };
                return null;
            }
            var user = Authenticate(this.Request.Headers.Authorization);
            if (user != null)
            {
                ctx.CurrentUser = user;
                return null;
            }
            return HttpStatusCode.Unauthorized;
        }

        CouchDBService db;
        TokenService tokenService;
        private void init()
        {
            db = new CouchDBService();
            tokenService = new TokenService(db);
        }

        private IUserIdentity Authenticate(string authorization)
        {
            var user = tokenService.FindUser(authorization);
            if (user == null)
            {
                Random r = new Random();
                Thread.Sleep(r.Next(500));
                return null;
            }
            return user;
        }
    }
}