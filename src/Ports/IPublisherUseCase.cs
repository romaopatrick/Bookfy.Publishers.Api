using Bookfy.Publishers.Api.Boundaries;
using Bookfy.Publishers.Api.Domain;
using Bookfy.Publishers.Api.Boundaries;

namespace Bookfy.Publishers.Api.Ports;
public interface IPublisherUseCase
{
    Task<Result<Publisher>> Create(CreatePublisher input, CancellationToken ct);
    Task<Result<Publisher>> Update(UpdatePublisher input, CancellationToken ct);
    Task<Result<Paginated<Publisher>>> Get(SearchPublishers input, CancellationToken ct);
    Task<Result<Publisher>> GetById(GetPublisherById input, CancellationToken ct);
    Task<Result> Delete(DeletePublisher input, CancellationToken ct);
}