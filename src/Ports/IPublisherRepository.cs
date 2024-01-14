using System.Linq.Expressions;
using Bookfy.Publishers.Api.Boundaries;
using Bookfy.Publishers.Api.Domain;

namespace Bookfy.Publishers.Api.Ports;

public interface IPublisherRepository
{
    Task<long> Count(Expression<Func<Publisher, bool>> predicate, CancellationToken ct);
    Task<Publisher> First(Expression<Func<Publisher, bool>> predicate, CancellationToken ct);
    Task<Paginated<Publisher>> Get(Expression<Func<Publisher, bool>> filter, long skip, long take, CancellationToken ct);
    Task<Publisher> Create(Publisher publisherPublisher, CancellationToken ct);
    Task<Publisher> Update(Publisher publisherPublisher, CancellationToken ct);
    Task Delete(Guid id, CancellationToken ct);
}