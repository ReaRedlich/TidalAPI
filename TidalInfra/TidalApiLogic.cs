using OpenTidl;
using OpenTidl.Methods;
using OpenTidl.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TidalInfra.Log;
using OpenTidl.Models.Base;
using System.Linq;

namespace TidalInfra
{
    public class TidalApiLogic
    {
        private readonly OpenTidlSession tidlSession;
        private readonly OpenTidlClient tidlClient;
        private readonly ConsoleLogger logger;

        public TidalApiLogic(OpenTidlSession tidlSession)
        {
            this.tidlSession = tidlSession;
            var defaultConfiguration = ClientConfiguration.Default;
            tidlClient = new OpenTidlClient(defaultConfiguration);
            this.logger = new ConsoleLogger();
        }

        public async Task<PlaylistModel> CreateUserPlaylistWithTitle(string title)
        {
            logger.Write(LogLevel.Info, $"The title is : {title}");
            var playlistModel = await tidlSession.CreateUserPlaylist(title);
            return playlistModel;
        }

        public async Task<EmptyModel> AddPlaylistTracks(string uuid, string playlistETag, IEnumerable<int> trackIds)
        {

            return await tidlSession.AddPlaylistTracks(uuid, playlistETag, trackIds);
        }

        public async Task<bool> DeletePlaylistTracks(string uuid)
        {
            var current = await tidlSession.GetPlaylistTracks(uuid);
            var userPlayList = await tidlSession.GetUserPlaylists();

            var ListOfIds = userPlayList.Items.Select(x => x.Id).ToList();//Run on Object and take only ids

            var deleteResponse = await tidlSession.DeletePlaylistTracks(uuid, current.ETag, ListOfIds);

            if (!string.IsNullOrEmpty(deleteResponse.ETag))
            {
                logger.Write(LogLevel.Info, $"Deleted for tag {deleteResponse.ETag}, and uuid: {uuid}");
            }
            var reTryGet = await GetPlayListTracks(uuid);//After deleted i check that there is no playlisttracks
            var isDeleted = reTryGet == null || reTryGet.Items.Length == 0;
            return isDeleted;
        }

        private Task<JsonList<TrackModel>> GetPlayListTracks(string uuid)
        {
            return tidlSession.GetPlaylistTracks(uuid);
        }
    }
}
