using MetaBrainz.MusicBrainz;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MLS.MusicDatabase.MusicBrainz
{
    public struct MusicBrainzAccount
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Uri { get; set; }
    }
    class MusicBrainzSyncronizer
    {
        private static Query query = new Query("Music Library Syncronizer", "1.0", "mailto:kurgyis.p@gmail.com");
        public static Dictionary<string, List<string>> results;
        private Uri redirect = new Uri("http://localhost:5000/callback");
        private string accessToken;
        public static Form1 form;

        public async void testMethod()
        {
            var oa = new OAuth2();
            oa.ClientId = "BpXLc9ZEEBiIK7bY-f8wjgoDwcXeYwiI";
            var url = oa.CreateAuthorizationRequest(new Uri("https://localhost:5000/callback/"), AuthorizationScope.Collection | AuthorizationScope.Rating);
            System.Diagnostics.Process.Start(url.OriginalString);
            var loginError = await WaitForLogin(5000, TimeSpan.FromSeconds(60 * 5));
            if (loginError.Length == 0)
            {
                Console.WriteLine("Login Successful!");
            }
            else
            {
                Console.WriteLine(loginError);
            }
        }

        public Task<string> WaitForLogin(int port, TimeSpan timeout)
        {
            var tcs = new TaskCompletionSource<string>();

            var server = new EmbedIOAuthServer(redirect, port);
            server.AuthorizationCodeReceived += async (sender, response) =>
            {
                await server.Stop();

                accessToken = response.Code;

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

        //TODO: Login/Authentication
        public async void authorize(String userId, String clientSecret)
        {
            /*var oa = new OAuth2();
            var authorizationToken = oa.CreateAuthorizationRequest(OAuth2.OutOfBandUri, AuthorizationScope.Ratings | AuthorizationScope.Tags);
            var at = await oa.GetBearerTokenAsync(authorizationToken.ToString(), clientSecret, OAuth2.OutOfBandUri);
            query.BearerToken = at.AccessToken;*/
        }
        //TODO: Find music by SpotifyData
        public static async Task<List<string>> findMusic(SongInfo song, IProgress<int> progress)
        {
            query.UrlScheme = "http";
            List<string> foundSongs = new List<string>();
            var artists = await query.FindArtistsAsync(song.artistName, simple: true).ConfigureAwait(false);
            foreach(var artist in artists.Results)
            {
                if (!artist.Item.Name.Equals(song.artistName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                int browseLimit = 100;
                var works = await query.BrowseArtistRecordingsAsync(artist.Item.Id, limit: browseLimit).ConfigureAwait(false);
                for(int i = 0; i <= works.TotalResults; i += browseLimit)
                {
                    foreach (var work in works.Results)
                    {
                        if (work.Title.Equals(song.songName))
                        {
                            foundSongs.Add(work.Id.ToString());
                            Console.WriteLine("Artist: " + artist.Item.Name + ", Song: " + work.Title + ", MBID: " + work.Id.ToString());
                        }
                    }
                    works.Next();
                }
            }
            progress.Report(1);
            return foundSongs;
        }

        public static async void findSongs(object sender, EventArgs e)
        {
            Dictionary<string, SongInfo> songs = (sender as IMusicPlayerData).GetSyncSongs();
            results = new Dictionary<string, List<string>>();
            var progress = form.SetupProgressBar(songs.Count);
            foreach (string songId in songs.Keys)
            {
                List<string> foundSongs = await findMusic(songs[songId], progress);
                results.Add(songId, foundSongs);
            }
            foreach (var id in results.Keys)
            {
                if (results[id].Count >= 2)
                {
                    form.UpdateSRListBox(songs[id]);
                }
            }
        }
        //TODO: Create Collection
        //TODO: Add music to collection

        public static List<string> GetSearchResults(string id)
        {
            return results[id];
        }
    }
}
