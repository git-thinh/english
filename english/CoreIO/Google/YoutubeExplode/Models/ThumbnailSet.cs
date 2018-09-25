﻿using JetBrains.Annotations;
using YoutubeExplode.Internal;

namespace YoutubeExplode.Models
{
    /// <summary>
    /// Set of thumbnails for a video.
    /// </summary>
    public class ThumbnailSet
    {
        private readonly string _videoId;

        /// <summary>
        /// Low resolution thumbnail URL.
        /// </summary>
        [NotNull]
        public string LowResUrl
        {
            get
            {
                return $"https://img.youtube.com/vi/{_videoId}/default.jpg";
            }
            set { }
        }

        /// <summary>
        /// Medium resolution thumbnail URL.
        /// </summary>
        [NotNull]
        public string MediumResUrl {
            get { return $"https://img.youtube.com/vi/{_videoId}/mqdefault.jpg"; }
            set { }
        }

        /// <summary>
        /// High resolution thumbnail URL.
        /// </summary>
        [NotNull]
        public string HighResUrl
        {
            get { return $"https://img.youtube.com/vi/{_videoId}/hqdefault.jpg"; }
            set { }
        }

        /// <summary>
        /// Standard resolution thumbnail URL.
        /// Not always available.
        /// </summary>
        [NotNull]
        public string StandardResUrl
        {
            get { return $"https://img.youtube.com/vi/{_videoId}/sddefault.jpg"; }
            set { }
        }

        /// <summary>
        /// Max resolution thumbnail URL.
        /// Not always available.
        /// </summary>
        [NotNull]
        public string MaxResUrl
        {
            get { return $"https://img.youtube.com/vi/{_videoId}/maxresdefault.jpg"; }
            set { }
        }

        public ThumbnailSet() { }

        /// <summary />
        public ThumbnailSet(string videoId)
        {
            _videoId = videoId.GuardNotNull(nameof(videoId));
        }
    }
}