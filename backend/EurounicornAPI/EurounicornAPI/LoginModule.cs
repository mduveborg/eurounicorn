using EurounicornAPI.Authentication;
using EurounicornAPI.Mailing;
using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace EurounicornAPI
{
    public class LoginRequestDto
    {
        public string Username { get; set; }
    }

    public class LoginModule : NancyModule
    {
        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public LoginModule()
            : base("/api/login")
        {
            var db = new CouchDB.CouchDBService();
            var tokenService = new TokenService(db);

            Post["/"] = ctx =>
            {
                string username;
                var dto = this.Bind<LoginRequestDto>();
                username = dto.Username;
                var validationResponse = ValidateUsername(username);
                if (validationResponse != null) return validationResponse;
                var token = tokenService.Login(username);
                var combine = string.Format("{0}!{1}", username, token);
                MailgunService.SendMail(username, "The unicorn says hi!", string.Format(File.ReadAllText(Path.Combine(AssemblyDirectory, "Mailing\\LoginMailResponse.txt")), this.Request.Url.SiteBase, combine));
                return HttpStatusCode.OK;
            };
        }

        private Response ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return HttpStatusCode.BadRequest;
            if (!Regex.IsMatch(username, ".?@netlight.?"))
                return HttpStatusCode.BadRequest;
            return null;
        }
    }
}