namespace BusJourneys.Core.Models.Responses;

public class GetCookiesDto
{
    public string? From { get; set; }
    public string? To { get; set; }
    public string? Date { get; set; }
}