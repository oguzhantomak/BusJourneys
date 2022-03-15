namespace BusJourneys.Core.Models.Responses;

public class SearchItemsDto
{
    public List<GetBusLocationsResponseDto.DataDto> items { get; set; }
    public int total_count { get; set; }
}