using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenTidl.Methods;
using TidalInfra;
using TidalInfra.Log;

namespace TidalTests.E2eTests
{
    [TestFixture]
    class E2eApiTests
    {
        private LoginLogic loginLogic;
        private readonly ConsoleLogger logger;

        public E2eApiTests()
        {
            this.logger = new ConsoleLogger();
        }

        [SetUp]
        public void InitTest()
        {
            loginLogic = new LoginLogic();
            var client = loginLogic.GetClient();
            Assert.IsNotNull(client);
        }

        [Test]
        public async Task FullFlowTest()
        {
            OpenTidlSession tidlSession = await loginLogic.BaseLogin();
            var tidalApiLogic = new TidalApiLogic(tidlSession);
            string title = $"Test_{Guid.NewGuid()}";
            logger.Write(LogLevel.Info, $"The title is : {title}");
            var res = await tidalApiLogic.CreateUserPlaylistWithTitle(title);
            var indices = new List<int> { 126462757 };//from actual website
            await tidalApiLogic.AddPlaylistTracks(res.Uuid, res.ETag, indices);
            var response = await tidalApiLogic.DeletePlaylistTracks(res.Uuid);
            Assert.AreEqual(response, true);
        }
    }
}
