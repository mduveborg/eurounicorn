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
    public class AuthEvent
    {
        public DateTime At
        {
            get;
            set;
        }

        public string Username { get; set; }

        public string docType { get { return "authEvent"; } }
    }
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
            var user = Authenticate(this.Request.Headers.Authorization);
            if (user != null)
            {
                ctx.CurrentUser = user;
                db.Set(new AuthEvent() { At = DateTime.UtcNow, Username = user.UserName });
                return null;
            }
            return HttpStatusCode.Unauthorized;
        }

        protected CouchDBService db;
        TokenService tokenService;
        private void init()
        {
            db = new CouchDBService();
            tokenService = new TokenService(db);
        }

        private IUserIdentity Authenticate(string authorization)
        {
            var splits = authorization.Split(new char[1] { '!' }, StringSplitOptions.RemoveEmptyEntries);
            var username = splits.First();
            var token = splits.Last();
            var user = tokenService.FindUser(token, username);
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