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
            MusicPlayerSelector.form = this;
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
                MusicPlayerSelector.PlayerSelected((MusicPlayer)(musicPlayerListBox.SelectedItem));
            }
        }

        public void ShowLogin()
        {
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
            playlistsListBox.Show();
            syncButton.Show();
        }

        public void HidePlaylists()
        {
            playlistsListBox.Hide();
            syncButton.Hide();
        }

        public void UpdatePLListBox(IList<PlaylistInfo> playlists)
        {
            foreach(PlaylistInfo pl in playlists)
            {
                playlistsListBox.Items.Add(pl);
            }
            Update();
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            if (playlistsListBox.SelectedItems.Count == 0)
            {
                return;
            }
            //Run database search on these
            MusicPlayerSelector.GetSongs(playlistsListBox.SelectedItems);
        }
    }
}
