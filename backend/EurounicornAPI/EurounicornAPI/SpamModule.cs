using EurounicornAPI.Authentication;
using EurounicornAPI.CouchDB;
using EurounicornAPI.Mailing;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace EurounicornAPI
{
    public class SpamModule : AuthModule
    {
        public SpamModule() : base("api/spam")
        {
            this.Post["/", true] = (_, cancel) =>
            {
                return Task.Run<dynamic>(() =>
                {
                    var file = this.Request.Files.FirstOrDefault();
                    string data;
                    using (StreamReader sr = new StreamReader(file.Value))
                    {
                        data = sr.ReadToEnd();
                    }
                    string username = this.Context.CurrentUser.UserName;
                    bool isValid = IsValidSpamUser(username);
                    var db = new CouchDBService();
                    var tokens = new TokenService(db);
                    string subject = this.Request.Form.subject;
                    string body = this.Request.Form.body;
                    foreach (var email in GetValidEmails(data, username, isValid))
                    {
                        var modBody = InsertLink(body, email, tokens);
                        MailgunService.SendMail(email, subject, modBody);
                    }
                    return HttpStatusCode.OK;
                });
            };
        }

        private string InsertLink(string body, string email, TokenService tokens)
        {
            if (body.IndexOf("!!link!!") != -1)
            {
                var token = tokens.Login(email);
                var combine = string.Format("{0}!{1}", email, token);
                var link = string.Format(@"<a href=""{0}/#/?token={1}"">your secret key</a>", this.Request.Url.SiteBase, combine);
                return body.Replace("!!link!!", link) + "<br><br> Sent on behalf of " + email;
            }
            return body;
        }

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

        private IEnumerable<string> GetValidEmails(string data, string owner, bool isValid)
        {
            return data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Where(s => Regex.IsMatch(s, "@netlight."))
                .Where(s => isValid || owner.Equals(s));
        }

        private bool IsValidSpamUser(string email)
        {
            if (email.Equals("daniel.werthen@netlight.com"))
                return true;
            return false;
        }
    }
}