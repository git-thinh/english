using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using YoutubeExplode.Internal;

namespace YoutubeExplode.Models
{
    /// <summary>
    /// Information about a YouTube video.
    /// </summary>
    public class Video
    {
        /// <summary>
        /// ID of this video.
        /// </summary>
        [NotNull]
        public string Id { get; set; }

        /// <summary>
        /// Author of this video.
        /// </summary>
        [NotNull]
        public string Author { get; set; }

        /// <summary>
        /// Upload date of this video.
        /// </summary>
        public DateTimeOffset UploadDate { get; set; }

        /// <summary>
        /// Title of this video.
        /// </summary>
        [NotNull]
        public string Title { get; set; }

        /// <summary>
        /// Description of this video.
        /// </summary>
        [NotNull]
        public string Description { get; set; }

        /// <summary>
        /// Thumbnails of this video.
        /// </summary>
        [NotNull]
        public ThumbnailSet Thumbnails { get; set; }

        /// <summary>
        /// Duration of this video.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Search keywords of this video.
        /// </summary>
        [NotNull, ItemNotNull]
        public List<string> Keywords { get; set; }

        /// <summary>
        /// Statistics of this video.
        /// </summary>
        [NotNull]
        public Statistics Statistics { get; set; }

        public Video() { }

        /// <summary />
        public Video(string id, string author, DateTimeOffset uploadDate, string title, string description,
            ThumbnailSet thumbnails, TimeSpan duration, List<string> keywords, Statistics statistics)
        {
            Id = id.GuardNotNull(nameof(id));
            Author = author.GuardNotNull(nameof(author));
            UploadDate = uploadDate;
            Title = title.GuardNotNull(nameof(title));
            Description = description.GuardNotNull(nameof(description));
            Thumbnails = thumbnails.GuardNotNull(nameof(thumbnails));
            Duration = duration.GuardNotNegative(nameof(duration));
            Keywords = keywords.GuardNotNull(nameof(keywords));
            Statistics = statistics.GuardNotNull(nameof(statistics));
        }

        /// <inheritdoc />
        public override string ToString() => Title;
    }
}