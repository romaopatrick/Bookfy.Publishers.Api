using System.Text.Json.Serialization;

namespace Bookfy.Publishers.Api.Boundaries
{
    public class UpdatePublisher
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string? CompanyName { get; set; }
        public string? TradeName { get; set; }
        public IDictionary<string, string>? Settings { get; set; }
        public string? Document { get; set; }
    }
}