using EurounicornAPI.Voting.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurounicornAPI.CouchDB
{
	public interface ICouchDBService
	{
		string Set<T>(T value) where T : class;

		T Get<T>(string id) where T : class;

		IEnumerable<T> FindByUsername<T>(string username) where T : class;

		IEnumerable<JToken> GetByDocType<T>(string docType) where T : class;

		IEnumerable<T> FindByTrackId<T>(int trackId) where T : class;

		IEnumerable<User> FindUsersByLevel(Level level);

		void Delete(IEnumerable<JToken> tokens);

		void Delete(CouchObject obj);
	}
}
