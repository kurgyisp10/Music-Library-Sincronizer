using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLS
{
    public enum MusicPlayer
    {
        Spotify
    }

    public static class MusicPlayerSelector
    {
        private static IMusicPlayerData musicPlayerData;

        public static Form1 form;

        public static void PlayerSelected(MusicPlayer musicPlayer)
        {
            
            switch (musicPlayer)
            {
                case MusicPlayer.Spotify:
                    {
                        musicPlayerData = new SpotifyData();
                        musicPlayerData.LoginResult += mp_LoginResult;
                        musicPlayerData.SyncSongsResult += MusicDatabase.MusicBrainz.MusicBrainzSyncronizer.findSongs;
                        musicPlayerData.PlaylistInfoResult += mp_PlaylistResult;
                        musicPlayerData.LoginAsync();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return;
        }

        public static void GetSongs(System.Windows.Forms.ListBox.SelectedObjectCollection selectedItems)
        {
            musicPlayerData.GetSyncSongsAsync(selectedItems);
            form.HidePlaylists();
            form.ShowSearchResolve();
            return;
        }

        private static void mp_LoginResult(object sender, bool e)
        {
            if (e)
            {
                IMusicPlayerData playerData = (sender as IMusicPlayerData);
                playerData.GetPlaylistInfoAsync();
                form.HideLogin();
                form.toggleSelectorButton(true);
                form.ShowPlaylists();
            } 
            else
            {
                form.HidePlaylists();
                form.ShowLogin();
            }
            
        }

        private static void mp_PlaylistResult(object sender, EventArgs e)
        {
            IMusicPlayerData playerData = (sender as IMusicPlayerData);
            form.UpdatePLListBox(playerData.GetPlaylistsInfo());
            
        }
    }
}
