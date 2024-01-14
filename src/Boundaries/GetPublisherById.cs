using Microsoft.AspNetCore.Mvc;

namespace Bookfy.Publishers.Api.Boundaries;

public class GetPublisherById
{
    [FromRoute(Name = "id")]
    public Guid Id { get; set; }
}