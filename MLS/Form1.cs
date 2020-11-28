using MLS.MusicDatabase.MusicBrainz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HidePlaylists();
            HideSearchResolve();
            HideConflict();
            MusicPlayerSelector.form = this;
            MusicBrainzSyncronizer.form = this;
            Array values = Enum.GetValues(typeof(MusicPlayer));
            foreach (MusicPlayer val in values)
            {
                musicPlayerListBox.Items.Add(val);
            }
        }

        private void selectorButton_Click(object sender, EventArgs e)
        {
            if (musicPlayerListBox.SelectedItem != null)
            {
                selectorButton.Enabled = false;
                tipLabel.Text = "Waiting for Login...";
                MusicPlayerSelector.PlayerSelected((MusicPlayer)(musicPlayerListBox.SelectedItem));
            }
            else
            {
                tipLabel.Text = "Select a service from the list above.";
            }
        }

        public void ShowLogin()
        {
            tipLabel.Text = "Select music service to synchronize.";
            musicPlayerListBox.Show();
            selectorButton.Show();
        }

        public void HideLogin()
        {
            musicPlayerListBox.Hide();
            selectorButton.Hide();
        }

        public void ShowPlaylists()
        {
            tipLabel.Text = "Select which playlists you want to synchronize. Use Ctrl to select multiple or Shift to select a range.";
            backToLoginBt.Show();
            playlistsListBox.Show();
            syncButton.Show();
        }

        public void HidePlaylists()
        {
            syncButton.Enabled = false;
            playlistsListBox.Hide();
            backToLoginBt.Hide();
            syncButton.Hide();
        }

        public void HideSearchResolve()
        {
            backToListsBt.Hide();
            songConflictsListBox.Hide();
        }

        public void ShowSearchResolve()
        {
            tipLabel.Text = "Couldn't find exact match for these songs. Double click on a song to resolve the search. Unresolved songs will be ignored.";
            backToListsBt.Show();
            songConflictsListBox.Show();
        }

        public void HideConflict()
        {
            conflictResolverListBox.Hide();
        }

        public void toggleSelectorButton(bool b)
        {
            selectorButton.Enabled = b;
        }

        public void toggleSyncButton(bool b)
        {
            syncButton.Enabled = b;
        }

        public void UpdatePLListBox(IList<PlaylistInfo> playlists)
        {
            foreach(PlaylistInfo pl in playlists)
            {
                playlistsListBox.Items.Add(pl);
            }
            Update();
            syncButton.Enabled = true;
        }

        public void UpdateSRListBox(SongInfo song)
        {
            songConflictsListBox.Items.Add(song);
            Update();
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            if (playlistsListBox.SelectedItems.Count == 0)
            {
                tipLabel.Text = "Select at least one playlist from the list above.";
                return;
            }
            //Run database search on these
            MusicPlayerSelector.GetSongs(playlistsListBox.SelectedItems);
        }

        private void backToLoginBt_Click(object sender, EventArgs e)
        {
            HidePlaylists();
            playlistsListBox.Items.Clear();
            ShowLogin();
        }

        private void songConflictsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.songConflictsListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                //TODO Show resolve list
            }
        }

        private void backToListsBt_Click(object sender, EventArgs e)
        {
            HideSearchResolve();
            syncButton.Enabled = true;
            ShowPlaylists();
        }
    }
}
