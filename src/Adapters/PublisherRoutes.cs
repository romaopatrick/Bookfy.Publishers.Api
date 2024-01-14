using System.Net;
using Bookfy.Publishers.Api.Boundaries;
using Bookfy.Publishers.Api.Ports;
using Bookfy.Publishers.Api.src.Boundaries;
using Microsoft.AspNetCore.Mvc;

namespace Bookfy.Publishers.Api.Adapters;

public static class PublisherRoutes
{
    public static RouteGroupBuilder MapPublisherRoutes(this RouteGroupBuilder app)
    {
        var group = app.MapGroup("publisher");

        group.MapGet("search",
            async (CancellationToken ct,
                IPublisherUseCase useCase,
                [AsParameters] SearchPublishers input) => 
                {
                    var result = await useCase.Get(input, ct);
                    return JsonFromResult(result);
                });

        group.MapPost("", 
            async (CancellationToken ct,
                IPublisherUseCase useCase,
                [FromBody] CreatePublisher input) =>
            {
                var result = await useCase.Create(input, ct);
                return JsonFromResult(result);
            });

        group.MapPut("{id:guid}",
            async (CancellationToken ct,
                IPublisherUseCase useCase,
                [FromRoute] Guid id,
                [FromBody] UpdatePublisher input) =>
                {
                    var result = await useCase.Update(input, ct);
                    return JsonFromResult(result);
                });

        return app;
    }

    private static IResult JsonFromResult<T>(Result<T> result) =>
           result.Code == (int)HttpStatusCode.NoContent
               ? Results.NoContent()
               : Results.Json(
                   data: result,
                   statusCode: result.Code);
}