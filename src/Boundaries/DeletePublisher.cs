using Microsoft.AspNetCore.Mvc;

namespace Bookfy.Publishers.Api.Boundaries;

public class DeletePublisher
{
    [FromRoute(Name = "id")]
    public Guid Id { get; set; }
}