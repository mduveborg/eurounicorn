using EurounicornAPI.Authentication;
using EurounicornAPI.SoundCloud;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using Nancy.Testing;

namespace EurounicornAPI.Tests
{
    [TestClass]
    public class VotingModuleTests
    {
        [TestMethod]
        public void TestCallingServiceFromModule()
        {

            //var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(cfg =>
            {
                cfg.Module<VotingModule>();
                cfg.RequestStartup((container, pipelines, context) =>
                {
                    context.CurrentUser = new UserIdentity { UserName = "patric.ogren@netlight.com" };
                });
            });

            // When
            var result = browser.Post("/api/vote", with =>
            {
                with.FormValue("Username", "patric.ogren@netlight.com");
                with.FormValue("Points", "2");
                with.FormValue("TrackId", "123456789");
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
