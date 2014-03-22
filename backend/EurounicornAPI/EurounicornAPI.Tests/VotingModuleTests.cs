using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using Nancy.Testing;

namespace EurounicornAPI.Tests
{
    [TestClass]
    public class VotingModuleTests
    {
        [TestMethod]
        public void TestGettingVotes()
        {
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Post("/api/vote", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
