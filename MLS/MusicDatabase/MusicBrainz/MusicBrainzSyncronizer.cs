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
    public struct CollectionInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> elements { get; set; }
    }
    class MusicBrainzSyncronizer
    {
        private static Query query = new Query("Music Library Syncronizer", "1.0", "mailto:kurgyis.p@gmail.com");
        public static Dictionary<string, List<string>> results;
        private static Dictionary<string, SongInfo> songsToSync;
        private Uri redirect = new Uri("https://localhost:5000/callback/");
        string clientSecret = "swGRYqGEo1xLYOpjl-zP5ZuuBlrGRu4M";
        string clientId = "BpXLc9ZEEBiIK7bY-f8wjgoDwcXeYwiI";
        bool authorizationTokenGranted = false;
        private string authorizationToken;
        static HttpListener _httpListener = new HttpListener();
        private static readonly HttpClient client = new HttpClient();
	    private string accessToken;
        public static Form1 form;
        public static string userName;

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

        public static async Task<List<string>> findMusic(SongInfo song, IProgress<int> progress)
        {
            query.UrlScheme = "http";
            List<string> foundSongs = new List<string>();
            var artists = await query.FindArtistsAsync(song.artistName, simple: true).ConfigureAwait(false);
            foreach (var artist in artists.Results)
            {
                if (!artist.Item.Name.Equals(song.artistName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                int browseLimit = 100;
                var works = await query.FindRecordingsAsync("arid:" + artist.Item.Id.ToString() + " AND \"" + song.songName + "\"", browseLimit);//query.BrowseArtistRecordingsAsync(artist.Item.Id, limit: browseLimit).ConfigureAwait(false);
                for (int i = 0; i <= works.TotalResults; i += browseLimit)
                {
                    foreach (var work in works.Results)
                    {
                        if (work.Item.Title.Equals(song.songName))
                        {
                            foundSongs.Add(work.Item.Id.ToString());
                            Console.WriteLine("Artist: " + artist.Item.Name + ", Song: " + work.Item.Title + ", MBID: " + work.Item.Id.ToString());
                        }
                    }
                    works.Next();
                }
            }
            progress.Report(1);
            return foundSongs;
        }

        public static List<string> GetSearchResults(string id)
        {
            return results[id];
        }

        public static async void findSongs(object sender, EventArgs e)
        {
            songsToSync = (sender as IMusicPlayerData).GetSyncSongs();
            results = new Dictionary<string, List<string>>();
            var progress = form.SetupProgressBar(songsToSync.Count);
            foreach (string songId in songsToSync.Keys)
            {
                List<string> foundSongs = await findMusic(songsToSync[songId], progress);
                results.Add(songId, foundSongs);
            }
            foreach (var id in results.Keys)
            {
                if (results[id].Count >= 2)
                {
                    form.UpdateSRListBox(songsToSync[id]);
                }
            }
            form.CheckConflicts();
        }

        public static void ClearResults()
        {
            results.Clear();
        }

        public static void selectMBID(string songId, string MBID)
        {
            results[songId].Clear();
            results[songId].Add(MBID);
        }
    
        private async Task<Dictionary<Guid, CollectionInfo>> getUserCollections(string user)
        {
            Dictionary<Guid, CollectionInfo> userCollections = new Dictionary<Guid, CollectionInfo>();
            int browseLimit = 50;
            var collections = await query.BrowseEditorCollectionsAsync(user, limit: browseLimit).ConfigureAwait(false);
            //var collections = query.BrowseEditorCollections(user, limit: browseLimit);
            for (int i = 0; i <= collections.TotalResults; i += browseLimit)
            {
                foreach (var collection in collections.Results)
                {
                    if(collection.Type != EntityType.Recording.ToString())
                    {
                        continue;
                    }
                    CollectionInfo collectionInfo = new CollectionInfo();
                    collectionInfo.Id = collection.Id;
                    collectionInfo.Name = collection.Name;
                    collectionInfo.elements = new List<Guid>();
                    var recordings = await query.BrowseCollectionRecordingsAsync(collection.Id, limit: browseLimit).ConfigureAwait(false);
                    //var recordings = query.BrowseCollectionRecordings(collection.Id, limit: browseLimit);
                    for (int j = 0; j <= recordings.TotalResults; j += browseLimit)
                    {
                        foreach (var recording in recordings.Results)
                        {
                            collectionInfo.elements.Add(recording.Id);
                            Console.WriteLine("Recording " + recording.Title + " (" + recording.Id + ") is in collection " + collection.Name + " (" + collection.Id + ").");
                        }
                        recordings.Next();
                    }
                    userCollections[collection.Id] = collectionInfo;
                }
                collections.Next();
            }
            return userCollections;
        }

        public async void syncronizeCollections()
        {
            Dictionary<Guid, CollectionInfo> collections = await getUserCollections(userName).ConfigureAwait(false);
            //Remove unnecessary songs from collection
            foreach(CollectionInfo collection in collections.Values)
            {
                bool collectionNeeded = false;
                foreach(SongInfo songInfo in songsToSync.Values)
                {
                    if (songInfo.playlists.Contains(collection.Name))
                    {
                        collectionNeeded = true;
                    }
                }
                if (collectionNeeded)
                {
                    foreach(Guid song in collection.elements)
                    {
                        bool songNeedsRemove = true;
                        string songMBIDInCollection = song.ToString();
                        foreach(string resultId in results.Keys)
                        {
                            if(results[resultId].Count >= 2)
                            {
                                //ERROR
                            }
                            else
                            {
                                string songMBIDFromSync = results[resultId][0];
                                if (songMBIDInCollection.Equals(songMBIDFromSync) && songsToSync[resultId].playlists.Contains(collection.Name))
                                {
                                    songNeedsRemove = false;
                                }
                            }
                        }
                        //Remove song from collection
                        //query.RemoveFromCollection(userName, collection.Id, EntityType.Recording, song);
                        Console.WriteLine("Song with MBID " + song.ToString() + " was removed from " + collection.Name + " (" + collection.Id + ")") ;
                    }
                }
            }
            
            //Add needed songs to collection
            foreach (string songId in results.Keys)
            {
                if (results[songId].Count >= 2)
                {
                    //ERROR
                }
                SongInfo songInfo = songsToSync[songId];
                Guid songMBID = new Guid(results[songId][0]);
                foreach(string playlist in songInfo.playlists)
                {
                    foreach(CollectionInfo collection in collections.Values)
                    {
                        if (playlist.Equals(collection.Name))
                        {
                            if (!collection.elements.Contains(songMBID))
                            {
                                //Add song to collection
                                //string result = await query.AddToCollectionAsync(userName, collection.Id, EntityType.Recording, new Guid(songMBID)).ConfigureAwait(false);
                                //Console.WriteLine(result);
                                Console.WriteLine(songInfo.songName + " from " + songInfo.artistName + " syncronized to " + collection.Name + " (" + collection.Id + ")");
                            }
                            else
                            {
                                //Song already in collection
                                Console.WriteLine(songInfo.songName + " from " + songInfo.artistName + " was already in " + collection.Name + " (" + collection.Id + ")");
                            }
                        }
                    }
                }
            }
        }
    }
}
