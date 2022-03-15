using System.Text.Json.Serialization;

namespace BusJourneys.Core.Models.Requests;

public class SessionCreateDto
{
    public int Type { get; set; }
    public Connection Connection { get; set; }
    public Browser Browser { get; set; }

}
public class Connection
{
    [JsonPropertyName("ip-address")]
    public string Ip { get; set; }
    public string Port { get; set; }
}

public class Browser
{
    [JsonPropertyName("version")]
    public string BrowserVersion { get; set; }

    [JsonPropertyName("name")]
    public string BrowserName { get; set; }
}