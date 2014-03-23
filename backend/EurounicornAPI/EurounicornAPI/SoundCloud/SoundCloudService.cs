using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ewk.SoundCloud.ApiLibrary.Entities;
using Ewk.SoundCloud.ApiLibrary;
using Ewk.Extensions;
using System.Net;
using System.IO;
using System.Text;
using Krystalware.UploadHelper;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Configuration;

namespace EurounicornAPI.SoundCloud
{
    public class UploadTrack
    {
        public Stream Data;
        public string Title;
        public string Filename;
    }

    public class SoundCloudService
    {
        string ClientSecret = "76c026455b3cb649c863594b26871cbe";
        string ClientId = "648b7a4aee93dd1a64337baf187e3eba";

        public string GetAccessToken()
        {
            //WebClient to communicate via http
            WebClient client = new WebClient();

            //Credentials (username & password)
			string username = ConfigurationManager.AppSettings["soundCloudUser"];
			string password = ConfigurationManager.AppSettings["soundCloudPassword"];

            //Authentication data
            string postData = "client_id=" + ClientId
                + "&client_secret=" + ClientSecret
                + "&grant_type=password&username=" + username
                + "&password=" + password;

            //Authentication
            string soundCloudTokenRes = "https://api.soundcloud.com/oauth2/token";
            string tokenInfo = client.UploadString(soundCloudTokenRes, postData);
            //Parse the token
            tokenInfo = tokenInfo.Remove(0, tokenInfo.IndexOf("token\":\"") + 8);
            var token = tokenInfo.Remove(tokenInfo.IndexOf("\""));
            return token;
        }

        private static string tooken;

        public UploadResponse Upload(UploadTrack track, string token = null)
        {
            if (token == null)
                token = GetAccessToken();
            var response = UploadTrack(track.Data, token, track.Title, track.Filename);
            
            return response;
        }


        public void DeleteTrack(string title, string token = null)
        {
            if (token == null)
                token = GetAccessToken();
            var cloud = new Ewk.SoundCloud.ApiLibrary.SoundCloud(ClientId, token);
            var tracks = cloud.Me.Tracks.Get();
            tracks.Where(t => t.title == title).ToList().ForEach(t => t.Delete());
        }

        public List<Track> GetTracks(string token = null, string title = null)
        {
            if (token == null)
                token = GetAccessToken();
            var cloud = new Ewk.SoundCloud.ApiLibrary.SoundCloud(ClientId, token);
            if (title != null)
                return cloud.Me.Tracks.Get().Where(t => t.title == title).ToList();
            return cloud.Me.Tracks.Get().ToList();
        }

        public string GetEmbedded(Track track, string token = null)
        {
            if (token == null)
                token = GetAccessToken();
            //WebClient to communicate via http
            WebClient client = new WebClient();
            var test = client.DownloadData(track.uri + ".json?" + "client_id=" + ClientId + "&oauth_token=" + token);


            //Authentication data
            string postData = "url=" + track.uri
                + "&format=json"
                + "&oauth_token=" + token;

            //Authentication
            string soundCloudTokenRes = "http://soundcloud.com/oembed";
            string tokenInfo = client.UploadString(soundCloudTokenRes, postData);
            return tokenInfo;
        }

        public IEnumerable<Track> GetTracks(string token)
        {
            var cloud = new Ewk.SoundCloud.ApiLibrary.SoundCloud(ClientId, token);
            return cloud.Me.Tracks.Get();
        }

        private UploadResponse UploadTrack(Stream data, string token, string title, string filename)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            var request = WebRequest.Create("https://api.soundcloud.com/tracks") as HttpWebRequest;
            //some default headers
            request.Accept = "*/*";
            request.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8,ru;q=0.6");

            //file array
            var files = new UploadFile[] { 
                new UploadFile(data, "track[asset_data]", filename, "application/octet-stream") 
            };
            //other form data
            var form = new NameValueCollection();
            form.Add("track[title]", title);
            form.Add("track[sharing]", "private");
            form.Add("oauth_token", token);
            form.Add("format", "json");


            form.Add("Filename", filename);
            form.Add("Upload", "Submit Query");
            try
            {
                using (var response = HttpUploadHelper.Upload(request, files, form))
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        return JsonConvert.DeserializeObject<UploadResponse>(reader.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class UploadResponse
    {
        public int id { get; set; }
    }
}