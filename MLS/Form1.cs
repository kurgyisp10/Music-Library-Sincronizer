using MLS.MusicDatabase.MusicBrainz;
using System;
using System.Collections.Generic;
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
            exitBt.Hide();
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
            usernameTextBox.Hide();
            backToListsBt.Hide();
            createListBt.Hide();
            songConflictsListBox.Hide();
        }

        public void ShowSearchResolve()
        {
            tipLabel.Text = "Searching database for selected songs.";
            usernameTextBox.Show();
            backToListsBt.Show();
            createListBt.Show();
            songConflictsListBox.Show();
        }

        public void HideConflict()
        {
            backToSearchConflictsBt.Hide();
            selectMBIDBt.Hide();
            conflictResolverListBox.Hide();
        }

        public void ShowConflict()
        {
            backToSearchConflictsBt.Show();
            selectMBIDBt.Show();
            conflictResolverListBox.Show();
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

        public Progress<int> SetupProgressBar(int max)
        {
            progressBar1.Show();
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            return new Progress<int>(v =>
            {
                progressBar1.PerformStep();
                if (progressBar1.Value == progressBar1.Maximum)
                {
                    progressBar1.Hide();
                    backToListsBt.Enabled = true;
                    createListBt.Enabled = true;
                }
            });
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            backToListsBt.Enabled = false;
            createListBt.Enabled = false;
            if (playlistsListBox.SelectedItems.Count == 0)
            {
                tipLabel.Text = "Select at least one playlist from the list above.";
                return;
            }
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
            tipLabel.Text = "Search for the recordings with these MBIDs on MusicBrainz to select the desired one.";
            int index = songConflictsListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                ResolveConflict((songConflictsListBox.Items[index] as SongInfo).songId);
            }
        }

        private void ResolveConflict(string songId)
        {
            HideSearchResolve();
            ShowConflict();
            conflictResolverListBox.Items.Clear();
            var MBIDList = MusicBrainzSyncronizer.GetSearchResults(songId);
            foreach (var mbid in MBIDList)
            {
                conflictResolverListBox.Items.Add(mbid);
            }
        }

        private void backToListsBt_Click(object sender, EventArgs e)
        {
            songConflictsListBox.Items.Clear();
            MusicBrainzSyncronizer.ClearResults();
            HideSearchResolve();
            syncButton.Enabled = true;
            ShowPlaylists();
        }

        private void backToSearchConflictsBt_Click(object sender, EventArgs e)
        {
            HideConflict();
            ShowSearchResolve();
            CheckConflicts();
        }

        private void selectMBIDBt_Click(object sender, EventArgs e)
        {
            if (conflictResolverListBox.SelectedItem == null)
            {
                tipLabel.Text = "Select which version you want to save.";
                return;
            }
            MusicBrainzSyncronizer.selectMBID((songConflictsListBox.SelectedItem as SongInfo).songId,
                conflictResolverListBox.SelectedItem.ToString());
            RefreshSRListBox();
            HideConflict();
            ShowSearchResolve();
            CheckConflicts();
        }

        private void RefreshSRListBox()
        {
            songConflictsListBox.Items.Remove(songConflictsListBox.SelectedItem);
        }

        public void CheckConflicts()
        {
            if (songConflictsListBox.Items.Count > 1)
            {
                tipLabel.Text = "These songs have multiple recordings in the database. Double click on a song to resolve the conflicts." +
                    " To create collection(s) without these songs write your username in the textbox below.";
            }
            else if (songConflictsListBox.Items.Count > 0)
            {
                tipLabel.Text = "This song has multiple recordings in the database. Double click on the song to resolve the conflict." +
                    " To create collection(s) without this song write your username in the textbox below.";
            }
            else
            {
                tipLabel.Text = "No conflicts found." +
                    " To create collection(s) write your username in the textbox below.";
            }
        }

        private async void createListBt_Click(object sender, EventArgs e)
        {
            if (usernameTextBox.TextLength == 0)
            {
                tipLabel.Text = "Please write your MusicBrainz Username in the textbox below.";
                return;
            }
            MusicBrainzSyncronizer.userName = usernameTextBox.Text;
            tipLabel.Text = "Updating collections.";
            createListBt.Enabled = false;
            backToListsBt.Enabled = false;
            await MusicBrainzSyncronizer.syncronizeCollections();
            HideSearchResolve();
            exitBt.Show();
            tipLabel.Text = "Collection data has been saved to a JSON file.";
        }

        private void exitBt_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
