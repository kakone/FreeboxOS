using System.Globalization;
using System.Text.Json.Serialization;

namespace FreeboxOS.TV;

/// <summary>
/// Stream
/// </summary>
public class Stream
{
    private string _url = null!;
    /// <summary>
    /// Gets or sets the url of the stream
    /// </summary>
    [JsonPropertyName("rtsp")]
    public string Url
    {
        get => _url;
        set
        {
            _url = value;
            UpdateQuality(value);
        }
    }

    /// <summary>
    /// Gets the quality of the stream
    /// </summary>
    [JsonIgnore]
    public Quality Quality { get; private set; }

    /// <summary>
    /// Gets or sets the text corresponding to the quality of the stream
    /// </summary>
    [JsonPropertyName("quality")]
    public string QualityText
    {
        get => Quality.ToString();
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                Quality = Quality.SD;
                return;
            }

            Quality = Enum.TryParse(value, true, out Quality q) ? q : Quality.SD;
            UpdateQuality(Url);
        }
    }

    private void UpdateQuality(string url)
    {
        if (url != null && CultureInfo.InvariantCulture.CompareInfo.IndexOf(url, "/fbxdvb/", CompareOptions.IgnoreCase) >= 0)
        {
            Quality = Quality == Quality.HD ? Quality.TNT_HD : Quality.TNT;
        }
    }
}
