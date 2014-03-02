using EurounicornAPI.CouchDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurounicornAPI.Tests
{
    [TestClass]
    public class CouchTests
    {
        public class Data
        {
            public string docType { get { return "Test"; } }
            public string value { get; set; }
        }

        [TestMethod]
        public void TestGetSetDocs()
        {
            var service = new CouchDBService();
            
            var toSet = new Data { value = "Hello world" };
            var setId = service.Set(toSet);
            var getResult = service.Get<Data>(setId);
            Assert.AreEqual(toSet.value, getResult.value);
            var allTests = service.GetByDocType<Data>(getResult.docType).ToList();
            Assert.IsTrue(allTests.Count() > 0);
            service.Delete(allTests);
            var notFound = service.Get<Data>(setId);
            Assert.IsNull(notFound);
        }
    }
}
