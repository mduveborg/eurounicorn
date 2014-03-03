using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    return HttpStatusCode.BadRequest;
                });
            };
        }
    }
}