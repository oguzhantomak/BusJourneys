using System.Text.Json.Serialization;

namespace BusJourneys.Core.Models.Responses;

public class GetBusJourneysResponseDto
{
    public string Status { get; set; }
    public List<DataDto> Data { get; set; }

    public class DataDto
    {
        public int Id { get; set; }

        // The property type in documentation is string, but it is actually an integer in Postman response
        [JsonPropertyName("partner-id")]
        public int? PartnerId { get; set; }

        [JsonPropertyName("partner-name")]
        public string? PartnerName { get; set; }

        [JsonPropertyName("route-id")]
        public int? RouteId { get; set; }

        [JsonPropertyName("available-seats")]
        public int? AvailableSeats { get; set; }

        public JourneyDto Journey { get; set; }
        public class JourneyDto
        {
            public JourneyDto()
            {
                Duration = TimeSpan.Zero;
            }
            public DateTime? Departure { get; set; }
            public DateTime? Arrival { get; set; }

            [JsonPropertyName("internet-price")]
            public decimal InternetPrice { get; set; }

            public string? Currency { get; set; }
            public TimeSpan? Duration { get; set; }
            public string? Origin { get; set; }
            public string? Destination { get; set; }

        }
    }
}