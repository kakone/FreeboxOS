using System;
using System.Text.Json.Serialization;

namespace FreeboxOS.TV
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subtitle
        /// </summary>
        [JsonPropertyName("sub_title")]
        public string? SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        [JsonPropertyName("category_name")]
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets the duration (in seconds)
        /// </summary>
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the start date (Unix timestamp)
        /// </summary>
        [JsonPropertyName("date")]
        public long Date { get; set; }

        private DateTimeOffset? _startDate;
        /// <summary>
        /// Gets the start date
        /// </summary>
        public DateTimeOffset StartDate => (DateTimeOffset)(_startDate ?? (_startDate = DateTimeOffset.FromUnixTimeSeconds(Date)));

        private DateTimeOffset? _endDate;
        /// <summary>
        /// Gets the end date
        /// </summary>
        public DateTimeOffset EndDate => (DateTimeOffset)(_endDate ?? (_endDate = StartDate.AddSeconds(Duration)));
    }
}
