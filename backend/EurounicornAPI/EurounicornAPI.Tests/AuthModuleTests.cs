using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using Nancy.Testing;
using System.Text;

namespace EurounicornAPI.Tests
{
    [TestClass]
    public class AuthModuleTests
    {
        [TestMethod]
        public void TestThatEmptyPost_BadRequest()
        {

            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Post("/api/login", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void TestThatNonNetlightPost_BadRequest()
        {

            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Post("/api/login", with =>
            {
                with.FormValue("Username", "daniel@kalle.com");
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void TestThatNetlightPost_OK()
        {
            return;
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Post("/api/login", with =>
            {
                with.FormValue("Username", "daniel.werthen@netlight.com");
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
