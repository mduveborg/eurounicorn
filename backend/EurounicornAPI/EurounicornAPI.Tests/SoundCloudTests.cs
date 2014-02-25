using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace EurounicornAPI.Tests
{
    [TestClass]
    public class SoundCloudTests
    {
        [TestMethod]
        public void TestGet()
        {
            /*
             * Something added
             */
            var service = new SoundCloud.SoundCloudService();
            var token = service.GetAccessToken();
            Assert.IsTrue(!string.IsNullOrEmpty(token));
            var file = "toddle.mp3";
            service.DeleteTrack(file, token);
            service.Upload(new SoundCloud.UploadTrack()
            {
                Title = file,
                Data = File.Open(file, FileMode.Open),
                Filename = file
            }, token);
            var tracks = service.GetTracks(token, file);
            Assert.IsTrue(tracks.Count == 1);
            service.DeleteTrack(file, token);
        }
    }
}
