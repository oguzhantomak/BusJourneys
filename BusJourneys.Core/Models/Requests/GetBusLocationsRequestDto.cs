using System.Text.Json.Serialization;

namespace BusJourneys.Core.Models.Requests;

public class GetBusLocationsRequestDto
{
    public string Data { get; set; }

    [JsonPropertyName("device-session")]
    public DeviceSessionDto DeviceSession { get; set; }

    public class DeviceSessionDto
    {
        [JsonPropertyName("device-id")]

        public string DeviceId { get; set; }


        [JsonPropertyName("session-id")]

        public string SessionId { get; set; }
    }

    public string Date { get; set; }
    public string Language { get; set; }
}