using Serilog;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MLS
{
    /*public struct SpotifyToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public DateTime CreatedAt { get; set; }
    }*/

    public struct SpotifyAccount
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Uri { get; set; }
    }

    public class SpotifyData : IMusicPlayerData
    {
        public string clientID = "9029c196ae8a4fc8902b9daa8b584d47";
        public IList<PlaylistInfo> playlistsInfo;

        private string state;
        private string verifier, challenge;
        private PKCETokenResponse token;
        private SpotifyAccount account;
        private SpotifyClient spotify;
        private Dictionary<string, SongInfo> songs;

        private Uri redirect;

        public SpotifyData()
        {
            state = Guid.NewGuid().ToString();
            redirect = new Uri("http://localhost:5000/callback");
            playlistsInfo = new List<PlaylistInfo>();
        }

        public event EventHandler<bool> LoginResult;
        public event EventHandler PlaylistInfoResult;
        public event EventHandler SyncSongsResult;

        public async Task GetPlaylistInfoAsync()
        {
            var authenticator = new PKCEAuthenticator(clientID, token);

            var config = SpotifyClientConfig.CreateDefault()
                                            .WithAuthenticator(authenticator);

            spotify = new SpotifyClient(config);

            var playlists = await spotify.PaginateAll(await spotify.Playlists.CurrentUsers()/*.ConfigureAwait(false)*/);
            Console.WriteLine($"Total Playlists in your Account: {playlists.Count}");
            

            var rand = await spotify.PaginateAll(await spotify.Library.GetTracks());
            Console.WriteLine($"Total Playlists in your Account: {rand.Count}");

            playlistsInfo.Add(new PlaylistInfo("0", "Liked Songs"));
            foreach (SimplePlaylist pl in playlists)
            {
                if (pl.Tracks.Total.GetValueOrDefault() > 0)
                {
                    playlistsInfo.Add(new PlaylistInfo(pl.Id, pl.Name));
                    Log.Information("Playlist: {@PlaylistInfo}", playlistsInfo[playlistsInfo.Count-1]);
                }
            }

            OnPlaylistInfoResult();
        }

        private void OnPlaylistInfoResult()
        {
            PlaylistInfoResult?.Invoke(this, EventArgs.Empty);
        }

        public async Task<string> LoginAsync()
        {
            (verifier, challenge) = PKCEUtil.GenerateCodes();
            var loginRequest = new LoginRequest(
                redirect,
                clientID, LoginRequest.ResponseType.Code
            )
            {
                State = state,
                CodeChallengeMethod = "S256",
                CodeChallenge = challenge,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative, Scopes.UserLibraryRead }
            };
            var uri = loginRequest.ToUri();
            BrowserUtil.Open(uri);

            var loginError = await WaitForLogin(5000, TimeSpan.FromSeconds(60 * 5), state);
            if (loginError.Length == 0)
            {
                Console.WriteLine("Login Successful!");
                OnLoginResult(true);
                
            }
            else
            {
                Console.WriteLine(loginError);
                OnLoginResult(false);
                Log.Information("Login Spotify Failed");
            }
            return "";
        }

        public Task<string> WaitForLogin(int port, TimeSpan timeout, string state)
        {
            var tcs = new TaskCompletionSource<string>();

            var server = new EmbedIOAuthServer(redirect, port);
            server.AuthorizationCodeReceived += async (sender, response) =>
            {
                await server.Stop();
                if (response.State != state)
                {
                    tcs.SetResult("Given state parameter was not correct.");
                    return;
                }

                token = await new OAuthClient().RequestToken(
                    new PKCETokenRequest(clientID, response.Code, redirect, verifier));

                var tempClient = new SpotifyClient(token.AccessToken);
                var user = await tempClient.UserProfile.Current();

                account.Id = user.Id;
                account.DisplayName = user.DisplayName;
                account.Uri = user.Uri;

                Log.Information("Login Spotify Succesful as " + user.DisplayName);
                server.Dispose();
                tcs.SetResult("");
            };

            var ct = new CancellationTokenSource(timeout);
            ct.Token.Register(() =>
            {
                server.Stop();
                server.Dispose();
                tcs.TrySetCanceled();
            }, useSynchronizationContext: false);

            server.Start();

            return tcs.Task;
        }

        protected virtual void OnLoginResult(bool IsSuccessful)
        {
            LoginResult?.Invoke(this, IsSuccessful);
        }

        public async Task GetSyncSongsAsync(System.Windows.Forms.ListBox.SelectedObjectCollection selectedItems)
        {
            songs = new Dictionary<string, SongInfo>();
            foreach(PlaylistInfo list in selectedItems)
            {
                if (list.PlaylistId.Equals("0"))
                {
                    var savedTracks = await spotify.PaginateAll(await spotify.Library.GetTracks());
                    foreach(SavedTrack st in savedTracks)
                    {
                        if (!songs.ContainsKey(st.Track.Id))
                        {
                            songs.Add(st.Track.Id, new SongInfo(st.Track.Name,
                                                                st.Track.Album.Name,
                                                                st.Track.Artists[0].Name,
                                                                st.Track.Album.ReleaseDate,
                                                                st.Track.Id));
                        }
                        songs[st.Track.Id].Playlists.Add("Spotify Liked Songs");
                    }
                }
                else
                {
                    var playlistTracks = await spotify.PaginateAll(await spotify.Playlists.GetItems(list.PlaylistId));
                    foreach (var pt in playlistTracks)
                    {
                        if (pt.Track.Type == ItemType.Track)
                        {
                            var t = (pt.Track as FullTrack);
                            if (!songs.ContainsKey(t.Id))
                            {
                                songs.Add(t.Id, new SongInfo(t.Name,
                                                             t.Album.Name,
                                                             t.Artists[0].Name,
                                                             t.Album.ReleaseDate,
                                                             t.Id));
                            }
                            songs[t.Id].Playlists.Add("Spotify " + list.PlaylistName);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            foreach (var item in songs.Values)
            {
                Log.Information("Song to sync: {@Song}", item);
            }
            OnSyncSongResult();
        }

        private void OnSyncSongResult()
        {
            SyncSongsResult?.Invoke(this, EventArgs.Empty);
        }

        public IList<PlaylistInfo> GetPlaylistsInfo()
        {
            return playlistsInfo;
        }

        public Dictionary<string, SongInfo> GetSyncSongs()
        {
            return songs;
        }
    }

}
