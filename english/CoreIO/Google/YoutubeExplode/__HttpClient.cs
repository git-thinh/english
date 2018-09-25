using System.IO;
using System.Net;

/*
    // Get data
    Video = await _client.GetVideoAsync(videoId);
    Channel = await _client.GetVideoAuthorChannelAsync(videoId);
    MediaStreamInfos = await _client.GetVideoMediaStreamInfosAsync(videoId);
    ClosedCaptionTrackInfos = await _client.GetVideoClosedCaptionTrackInfosAsync(videoId); 
*/

namespace YoutubeExplode.Internal
{
    public class HttpClient {

    }

    internal static class HttpClientEx
    {
        private static HttpClient _singleton;

        public static HttpClient GetSingleton()
        {
            // Return cached singleton if already initialized
            if (_singleton != null)
                return _singleton;

            //// Configure handler
            //var handler = new HttpClientHandler();
            //if (handler.SupportsAutomaticDecompression)
            //    handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //handler.UseCookies = false;

            // Configure client
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("User-Agent", "YoutubeExplode (github.com/Tyrrrz/YoutubeExplode)");

            return _singleton = client;
        }

        //public static HttpResponseMessage HeadAsync(this HttpClient client, string requestUri)
        //{
        //    using (var request = new HttpRequestMessage(HttpMethod.Head, requestUri))
        //        //return await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
        //        return client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
        //}

        public static string GetStringAsync(this HttpClient client, string requestUri,
            bool ensureSuccess = true)
        {
            ////using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
            //using (var response = client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead).Result)
            //{
            //    if (ensureSuccess)
            //        response.EnsureSuccessStatusCode();

            //    return response.Content.ReadAsStringAsync().Result; //.ConfigureAwait(false);
            //}
            return string.Empty;
        }

        public static Stream GetStreamAsync(this HttpClient client, string requestUri,
            long? from = null, long? to = null, bool ensureSuccess = true)
        {
            ////var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            ////request.Headers.Range = new RangeHeaderValue(from, to);

            ////using (request)
            ////{
            ////    //var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            ////    var response = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

            ////    if (ensureSuccess)
            ////        response.EnsureSuccessStatusCode();

            ////    return response.Content.ReadAsStreamAsync().Result;
            ////}
            return new MemoryStream();
        }
    }
}