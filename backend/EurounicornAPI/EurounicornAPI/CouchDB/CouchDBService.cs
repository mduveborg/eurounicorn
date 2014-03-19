using EurounicornAPI.Voting.Entities;
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
            
            database = client.GetDatabase("submissions_test");

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
                    byUsername = new 
                    {
                        Map = "function (doc) { if (doc.docType === 'token') { emit(doc.Username, doc); } }"
                    },
                    byTrackId = new
                    {
                        Map = "function(doc) { if (doc.docType === 'track') { emit(doc.TrackId, doc); } }"
                    },
                    byLevel = new
                    {
                        Map = "function(doc) { if (doc.docType === 'user') { emit(doc.Level, doc); } }"
                    }
                }
            };
            //var prev = database.GetDocument(doc._id);
            //if (prev != null)
            //    database.DeleteDocument(prev.Id, prev.Rev);
            //database.CreateDocument(doc._id, JsonConvert.SerializeObject(doc, Formatting.Indented, settings));


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

        public IEnumerable<T> FindByUsername<T>(string username) where T : class
        {
            var tokens = database.View<T>("byUsername", new ViewOptions
            {
                Stale = false,
                Key = new KeyOptions(username)
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

        public IEnumerable<T> FindByTrackId<T>(int trackId) where T : class
        {
            var trackMetaObjects = database.View<T>("byTrackId", new ViewOptions
            {
                Stale = false,
                Key = new KeyOptions(trackId)
            });
            return trackMetaObjects.Items;
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

        public IEnumerable<User> FindUserByLevel(Level level)
        {
            var usersWithLevel = database.View<User>("byLevel", new ViewOptions
            {
                Stale = false,
                Key = new KeyOptions(level)
            });
            return usersWithLevel.Items;
        }
    }
}