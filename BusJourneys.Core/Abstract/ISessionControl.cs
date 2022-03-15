using BusJourneys.Core.Models.Responses;

namespace BusJourneys.Core.Abstract;

public interface ISessionControl
{
    Task<List<GetBusLocationsResponseDto.DataDto>> GetBusLocations(string key);
    Task<List<GetBusJourneysResponseDto.DataDto>> GetBusJourneys(int from, int to, DateTime date);


}