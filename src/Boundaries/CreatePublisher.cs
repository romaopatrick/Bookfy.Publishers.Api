namespace Bookfy.Publishers.Api.Boundaries;
public class CreatePublisher
{
    public required string CompanyName { get; set; }
    public required string TradeName { get; set; }
    public required IDictionary<string, string> Settings { get; set; }
    public required string Document { get; set; }
}