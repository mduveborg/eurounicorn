using LoveSeat;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EurounicornAPI.CouchDB
{
    public class CouchObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string _id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string _rev { get; set; }
    }

    public class CouchDBService
    {
        CouchClient client;
        CouchDatabase database;
        static string designDoc = "submissions";

        public CouchDBService()
        {
            // connect to Cloudant
            client = new CouchClient("unieurocorn.cloudant.com", 443, "hermstaredgeshoreseembel", "36nDB1aax4CgOBgflrFgIpPU", true, AuthenticationType.Basic);
            
            database = client.GetDatabase("submissions");

            var settings = new JsonSerializerSettings();
            var converters = new List<JsonConverter> { new IsoDateTimeConverter() };
            settings.Converters = converters;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var doc = new
            {
                _id = "_design/" + designDoc,
                Language = "javascript",
                Views = new
                {
                    byDocType = new
                    {
                        Map = "function(doc) {\n  emit(doc.docType, doc);\n}"
                    },
                    byToken = new 
                    {
                        Map = "function (doc) { emit(doc.token, doc); }"
                    }
                }
            };
            var prev = database.GetDocument(doc._id);
            if (prev != null)
                database.DeleteDocument(prev.Id, prev.Rev);
            database.CreateDocument(doc._id, JsonConvert.SerializeObject(doc, Formatting.Indented, settings));


            database.SetDefaultDesignDoc(designDoc);
        }

        public string Set<T>(T value) where T : class
        {
            var json = JsonConvert.SerializeObject(value);
            var doc = database.CreateDocument(json);

            return doc.GetValue("id").ToObject<string>();
        }

        public T Get<T>(string id) where T : class
        {
            var doc = database.GetDocument(id);
            return doc != null ? doc.ToObject<T>() : null;
        }

        public IEnumerable<T> FindByToken<T>(string token) where T : class
        {
            var tokens = database.View<T>("byToken", new ViewOptions
            {
                Stale = false,
                Key = new KeyOptions(token)
            });
            return tokens.Items;
        }

        public IEnumerable<JToken> GetByDocType<T>(string docType) where T : class
        {
            var test = database.View("byDocType", new ViewOptions
            {
                Stale = false,
                Key = new KeyOptions(docType)
            });
            return test.Rows.Select(r => r.Value<JToken>("value")).ToList();
        }   

        public void Delete(IEnumerable<JToken> tokens)
        {
            foreach (var token in tokens)
            {
                database.DeleteDocument(token.Value<string>("_id"), token.Value<string>("_rev"));
            }
        }

        public void Delete(CouchObject obj)
        {
            database.DeleteDocument(obj._id, obj._rev);
        }
    }
}