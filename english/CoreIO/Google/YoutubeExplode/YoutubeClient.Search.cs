﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using YoutubeExplode.Internal;
using YoutubeExplode.Models;
using System.Web;

namespace YoutubeExplode
{
    public partial class YoutubeClient
    {
        public string GetStringAsync(string url, bool ensureSuccess = false)
        { 
            return _httpClient.GetStringAsync(url, ensureSuccess);
        }

        private string GetSearchResultsRawAsync(string query, int page = 1)
        {
            query = query.UrlEncode();
            // search_ajax?style=json&embeddable=1
            //var url = $"https://www.youtube.com/search_ajax?style=json&search_query={query}&page={page}&hl=en";
            var url = $"https://www.youtube.com/search_ajax?style=json&search_query={query}&page={page}&hl=en&sp=EgIoAQ%253D%253D"; 
            return _httpClient.GetStringAsync(url, false);
        }

        private  JToken  GetSearchResultsAsync(string query, int page = 1)
        {
            var raw = GetSearchResultsRawAsync(query, page);
            return JToken.Parse(raw);
        }

        /// <inheritdoc />
        public List<Video> SearchVideosAsync(string query, int maxPages)
        {
            query.GuardNotNull(nameof(query));
            maxPages.GuardPositive(nameof(maxPages));

            // Get all videos across pages
            var videos = new List<Video>();
            for (var i = 1; i <= maxPages; i++)
            {
                // Get search results
                var searchResultsJson =   GetSearchResultsAsync(query, i);

                // Get videos
                var videosJson = searchResultsJson["video"].EmptyIfNull().ToArray();

                // Break if there are no videos
                if (!videosJson.Any())
                    break;

                // Parse videos
                foreach (var videoJson in videosJson)
                {
                    // Basic info
                    var videoId = videoJson["encrypted_id"].Value<string>();
                    var videoAuthor = videoJson["author"].Value<string>();
                    //var videoUploadDate = videoJson["added"].Value<string>().ParseDateTimeOffset("M/d/yy");
                    var videoUploadDate = videoJson["added"].Value<string>().ParseDateTimeOffset();
                    var videoTitle = videoJson["title"].Value<string>();
                    var videoDuration = TimeSpan.FromSeconds(videoJson["length_seconds"].Value<double>());
                    var videoDescription = videoJson["description"].Value<string>().HtmlDecode();

                    // Keywords
                    var videoKeywordsJoined = videoJson["keywords"].Value<string>();
                    var videoKeywords = Regex
                        .Matches(videoKeywordsJoined, @"(?<=(^|\s)(?<q>""?))([^""]|(""""))*?(?=\<q>(?=\s|$))")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .Where(s => s.IsNotBlank())
                        .ToList();

                    // Statistics
                    var videoViewCount = videoJson["views"].Value<string>().StripNonDigit().ParseLong();
                    var videoLikeCount = videoJson["likes"].Value<long>();
                    var videoDislikeCount = videoJson["dislikes"].Value<long>();
                    var videoStatistics = new Statistics(videoViewCount, videoLikeCount, videoDislikeCount);

                    // Video
                    var videoThumbnails = new ThumbnailSet(videoId);
                    var video = new Video(videoId, videoAuthor, videoUploadDate, videoTitle, videoDescription,
                        videoThumbnails, videoDuration, videoKeywords, videoStatistics);

                    videos.Add(video);
                }
            }

            return videos;
        }

        /// <inheritdoc />
        public List<Video> SearchVideosAsync(string query)
        { 
            return SearchVideosAsync(query, int.MaxValue);
        }
    }
}