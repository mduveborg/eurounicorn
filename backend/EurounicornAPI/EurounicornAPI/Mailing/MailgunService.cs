using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.Mailing
{
    public class MailgunService
    {
        public static IRestResponse SendSimpleMessage()
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                   new HttpBasicAuthenticator("api",
                                              "key-2tm-xybgf7nkh-7yssk0ug34ciaj5ld9");
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                "eurounicorn.func.is", ParameterType.UrlSegment);
            request.Resource = "eurounicorn.func.is/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@eurounicorn.func.is>");
            request.AddParameter("to", "Daniel Werthén <daniel.werthen@netlight.com>");
            request.AddParameter("subject", "Hello Daniel Werthén");
            request.AddParameter("text", "Congratulations Daniel Werthén, you just sent an email with Mailgun!  You are truly awesome!  You can see a record of this email in your logs: https://mailgun.com/cp/log .  You can send up to 300 emails/day from this sandbox server.  Next, you should add your own domain so you can send 10,000 emails/month for free.");
            request.Method = Method.POST;
            return client.Execute(request);
        }

        public static IRestResponse SendMail(string email, string subject, string body)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                   new HttpBasicAuthenticator("api",
                                              "key-2tm-xybgf7nkh-7yssk0ug34ciaj5ld9");
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                "eurounicorn.func.is", ParameterType.UrlSegment);
            request.Resource = "eurounicorn.func.is/messages";
            request.AddParameter("from", "Rarity <rarity@eurounicorn.func.is>");
            request.AddParameter("to", email);
            request.AddParameter("subject", subject);
            request.AddParameter("text", body);
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}