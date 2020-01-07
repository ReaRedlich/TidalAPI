using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenTidl.Methods;
using TidalInfra;
using TidalInfra.Log;

namespace TidalTests.ComponentTests
{
    [TestFixture]
    public class LoginComponentTests
    {
        private LoginLogic loginLogic;
        private readonly ConsoleLogger logger;

        public LoginComponentTests()
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
        public async Task BasicLoginTest()
        {
            OpenTidlSession result = await loginLogic.BaseLogin();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task NegativeLoginTest()
        {
            string randomName = $"Test_{Guid.NewGuid().ToString()}";
            logger.Write(LogLevel.Info, $"The random name is : {randomName}");
            var result = await loginLogic.BaseLogin(randomName);
            logger.Write(LogLevel.Info, $"Status code is : {result.ErrorCode.Value}");
            Assert.AreEqual(result.ErrorCode.Value, 401);
        }
    }
}
