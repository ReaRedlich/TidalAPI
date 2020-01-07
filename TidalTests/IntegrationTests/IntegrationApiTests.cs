using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenTidl.Methods;
using TidalInfra;
using TidalInfra.Log;

namespace TidalTests.IntegrationTests
{
    [TestFixture]
    class IntegrationApiTests
    {
        private LoginLogic loginLogic;
        private readonly ConsoleLogger logger;

        public IntegrationApiTests()
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
		public async Task CreateUserPlaylistWithTitleTest()
		{
			var guid = Guid.NewGuid();//reusable as name might be unique
			string title = $"Test_{guid}";
			logger.Write(LogLevel.Info, $"The title is : {title}");
			OpenTidlSession tidlSession = await loginLogic.BaseLogin();
			var tidalApiLogic = new TidalApiLogic(tidlSession);
			var res = await tidalApiLogic.CreateUserPlaylistWithTitle(title);
			Assert.AreEqual(title, res.Title);
		}

		[Test]
		public async Task AddPlaylistTracksTest()
		{
			OpenTidlSession tidlSession = await loginLogic.BaseLogin();
			var tidalApiLogic = new TidalApiLogic(tidlSession);
			string title = $"Test_{Guid.NewGuid()}";//reusable as name might be unique
			logger.Write(LogLevel.Info, $"The title is : {title}");
			var res = await tidalApiLogic.CreateUserPlaylistWithTitle(title);
			var indices = new List<int> { 126462757 };//from actual website - not finded another way
			var addPlaylistTracksResponse = await tidalApiLogic.AddPlaylistTracks(res.Uuid, res.ETag, indices);
			var userPlayList = await tidlSession.GetUserPlaylists();
			Assert.AreEqual(userPlayList.ETag, addPlaylistTracksResponse.ETag);
		}
	}
}
