using EurounicornAPI.Authentication;
using EurounicornAPI.Mailing;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EurounicornAPI
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
            : base("/api/login")
        {
            var db = new CouchDB.CouchDBService();
            var tokenService = new TokenService(db);

            Post["/"] = ctx =>
            {
                var username = this.Request.Form.Username;
                var validationResponse = ValidateUsername(username);
                if (validationResponse != null) return validationResponse;
                var token = tokenService.Login(username);
                MailgunService.SendMail(username, "The unicorn says hi!", string.Format(File.ReadAllText("./Mailing/LoginMailResponse.txt"), "unieurocornlightvision.azurewebsites.net", token));
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