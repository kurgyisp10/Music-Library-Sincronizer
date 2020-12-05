using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLS
{
    public class SongInfo
    {
        private string songName;
        private string albumName;
        private string artistName;
        private string releaseDate;
        private string songId;
        private List<string> playlists;

        public string SongName { get => songName; set => songName = value; }
        public string AlbumName { get => albumName; set => albumName = value; }
        public string ArtistName { get => artistName; set => artistName = value; }
        public string ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public string SongId { get => songId; set => songId = value; }
        public List<string> Playlists { get => playlists; set => playlists = value; }

        public SongInfo(string songName, string albumName, string artistName, string releaseDate, string songId)
        {
            this.SongName = songName;
            this.AlbumName = albumName;
            this.ArtistName = artistName;
            this.ReleaseDate = releaseDate;
            this.SongId = songId;
            Playlists = new List<string>();
        }

        public override string ToString()
        {
            return SongName + " by " + ArtistName;
        }
    }
    public class PlaylistInfo
    {
        private string playlistId;
        private string playlistName;

        public string PlaylistId { get => playlistId; set => playlistId = value; }
        public string PlaylistName { get => playlistName; set => playlistName = value; }

        public PlaylistInfo(string playlistId, string playlistName)
        {
            this.PlaylistId = playlistId;
            this.PlaylistName = playlistName;
        }

        public override string ToString()
        {
            return PlaylistName;
        }
    }

    public interface IMusicPlayerData
    {
        Task<string> LoginAsync();
        Task GetPlaylistInfoAsync();
        Task GetSyncSongsAsync(System.Windows.Forms.ListBox.SelectedObjectCollection selectedItems);

        event EventHandler<bool> LoginResult;
        event EventHandler PlaylistInfoResult;
        event EventHandler SyncSongsResult;

        IList<PlaylistInfo> GetPlaylistsInfo();
        Dictionary<string, SongInfo> GetSyncSongs();
    }
}
