using System.Text.Json.Serialization;

namespace BusJourneys.Core.Models.Responses;

public class SessionGetDto
{
    public DataDto Data { get; set; }

    public class DataDto
    {
        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; }

        [JsonPropertyName("session-id")]
        public string SessionId { get; set; }
    }
}