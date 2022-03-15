namespace BusJourneys.Core.Models.Responses;

public class GetBusLocationsResponseDto
{
    public GetBusLocationsResponseDto()
    {
        //When the data is null, the default value is set to new list
        Data = new List<DataDto>();
    }

    public string Status { get; set; }
    public List<DataDto> Data { get; set; }

    //Response Data
    public class DataDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

    }
}