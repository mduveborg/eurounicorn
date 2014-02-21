using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy.Testing;
using Nancy;
using Nancy.Testing.Fakes;

namespace EurounicornAPI.Tests
{
    [TestClass]
    public class StatusOK
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.IsTrue(HttpStatusCode.OK == result.StatusCode);
        }
    }
}
