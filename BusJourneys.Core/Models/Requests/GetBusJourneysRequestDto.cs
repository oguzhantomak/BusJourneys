using System.Text.Json.Serialization;

namespace BusJourneys.Core.Models.Requests;

public class GetBusJourneysRequestDto
{
    [JsonPropertyName("device-session")]
    public DeviceSessionModel DeviceSession { get; set; }

    public string Date { get; set; }
    public string Language { get; set; }
    public DataDto Data { get; set; }

    public class DataDto
    {
        [JsonPropertyName("origin-id")]
        public int OriginId { get; set; }

        [JsonPropertyName("destination-id")]
        public int DestinationId { get; set; }

        [JsonPropertyName("departure-date")]
        public string DepartureDate { get; set; }
    }

    public class DeviceSessionModel
    {
        [JsonPropertyName("session-id")]
        public string SessionId { get; set; }

        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; }
    }
}