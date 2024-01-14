namespace Bookfy.Publishers.Api.Domain;

public class Publisher
{
    public Guid Id { get; set; }
    public required string CompanyName { get; set; }
    public required string TradeName { get; set; }
    public required IDictionary<string, string> Settings { get; set; }
    public required string Document { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}