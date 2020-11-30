using MetaBrainz.MusicBrainz;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Net;
using System.Net.Http;
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

        private Uri redirect = new Uri("https://localhost:5000/callback/");
        string clientSecret = "swGRYqGEo1xLYOpjl-zP5ZuuBlrGRu4M";
        string clientId = "BpXLc9ZEEBiIK7bY-f8wjgoDwcXeYwiI";
        bool authorizationTokenGranted = false;
        private string authorizationToken;
        static HttpListener _httpListener = new HttpListener();
        private static readonly HttpClient client = new HttpClient();
	private string accessToken;
        public static Form1 form;


        public void testMethod()
        {
            
        }

        //TODO: Login/Authentication
        public async void authorize()
        {
            // Setting up the webserver for authorization token callback
            _httpListener.Prefixes.Add(redirect.OriginalString);
            _httpListener.Start();
            Thread _responseThread = new Thread(() =>
            {
                HttpListenerContext context = _httpListener.GetContext();
                byte[] _responseArray = Encoding.UTF8.GetBytes("<html><head><title>Callback</title></head>" +
                "<body>Login success.</body></html>");
                context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length);
                var callbackUrl = context.Request.Url.OriginalString;
                authorizationToken = callbackUrl.Substring(callbackUrl.IndexOf('=') + 1);
                authorizationTokenGranted = true;
                context.Response.KeepAlive = false;
                context.Response.Close();
            });
            _responseThread.Start();
            // Getting the authorization token from MusicBrainz
            var oa = new OAuth2();
            oa.ClientId = clientId;
            var url = oa.CreateAuthorizationRequest(redirect, AuthorizationScope.Collection);
            System.Diagnostics.Process.Start(url.OriginalString);
            while (!authorizationTokenGranted)
            {
                Thread.Sleep(100);
            }

            // Getting access token using BearerToken (Sync)
            var at = oa.GetBearerToken(authorizationToken, clientSecret, redirect);
            query.BearerToken = at.AccessToken;
            Console.WriteLine("Access token: " + at.AccessToken);

            // Getting access token using BearerToken (Async)
            /*var at = await oa.GetBearerTokenAsync(authorizationToken, clientSecret, redirect);
            query.BearerToken = at.AccessToken;
            Console.WriteLine("Access token: " + at.AccessToken);*/

            // Alternative solution 1 sending direct POST request
            /*using (var wb = new WebClient())
            {
                wb.Encoding = Encoding.ASCII;
                var data = new System.Collections.Specialized.NameValueCollection();
                data["grant_type"] = "authorization_code";
                data["code"] = authorizationToken;
                data["clien_id"] = clientId;
                data["client_secret"] = clientSecret;
                data["redirect_uri"] = redirect.OriginalString;
                var response = wb.UploadValues("https://www.musicbrainz.org/oauth2/token", "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);
                Console.WriteLine(responseInString);
            }*/

            // Alternative solution 2 sending direct POST request
            /*var values = new Dictionary<string, string>
            {
                { "grant_type",  "authorization_code" },
                { "code", authorizationToken },
                { "clien_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirect.OriginalString }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://www.musicbrainz.org/oauth2/token", content);
            var responseString = await response.Result.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);*/
        }


        public static async void findSongs(object sender, EventArgs e)
        {
            Dictionary<string, SongInfo> songs = (sender as IMusicPlayerData).GetSyncSongs();
            results = new Dictionary<string, List<string>>();
            foreach (string songId in songs.Keys)
            {
                List<string> foundSongs = new List<string>();
                SongInfo song = songs[songId];
                var artists = query.FindArtists(song.artistName, simple: true);
                foreach (var artist in artists.Results)
                {
                    if (!artist.Item.Name.Equals(song.artistName))
                    {
                        continue;
                    }
                    int browseLimit = 100;
                    var works = await query.BrowseArtistWorksAsync(artist.Item.Id, limit: browseLimit);
                    for (int i = 0; i <= works.TotalResults; i += browseLimit)
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
    }
}
