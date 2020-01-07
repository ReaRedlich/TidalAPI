using OpenTidl;
using OpenTidl.Methods;
using OpenTidl.Transport;
using System;
using System.Threading.Tasks;
using TidalInfra.DTO;
using TidalInfra.Log;

namespace TidalInfra
{
    public class LoginLogic
    {
        private readonly OpenTidlClient openTidlClient;
        private const string UserName = @"danganon8520@gmail.com";
        private const string Password = @"Presley@1977";
        private readonly ConsoleLogger logger;


        /*
         * Every time there is a instance of this class, automatic he takes the default configuration
         */
        public LoginLogic()
        {
            this.logger = new ConsoleLogger();
            var defaultConfiguration = ClientConfiguration.Default;
            openTidlClient = new OpenTidlClient(defaultConfiguration);
        }


        /*
         * Because login api is async, i has await to make him sync
         */
        public async Task<OpenTidlSession> BaseLogin()
        {
            logger.Write(LogLevel.Info, $"Login activate for: {UserName} and {Password}");
            var loginObject = new LoginDto { Username = UserName, Password = Password };
            return await openTidlClient.LoginWithUsername(loginObject.Username, loginObject.Password);
        }

        /*
        * This method for sending incorrect UserName(Can do the same for password and another to send for both of the incorrect values)
        */
        public async Task<OpenTidlSessionExtendDto> BaseLogin(string UserName)
        {
            var response = new OpenTidlSessionExtendDto();
            try
            {
                logger.Write(LogLevel.Info, $"Login activate for: {UserName} and {Password}");
                var loginObject = new LoginDto { Password = Password };
                response.OpenTidlSession = await openTidlClient.LoginWithUsername(UserName, loginObject.Password);
                return response;
            }
            catch (Exception ex)
            {
                logger.WriteError($"Unauthorized, {ex.Message}");
                if (ex is OpenTidlException)
                {
                    var e = ex as OpenTidlException;
                    response.Error = e.OpenTidlError?.UserMessage;
                    response.ErrorCode = e.OpenTidlError?.Status;
                    logger.WriteError($"Get Exception because incorrect details, {ex.Message}");
                    return response;
                }

                response.Error = ex.Message;
                return response;
            }
        }

        public OpenTidlClient GetClient()
        {
            return openTidlClient;
        }
    }
}
