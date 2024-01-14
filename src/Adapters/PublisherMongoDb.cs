using System.Linq.Expressions;
using Bookfy.Publishers.Api.Boundaries;
using Bookfy.Publishers.Api.Domain;
using Bookfy.Publishers.Api.Ports;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bookfy.Publishers.Api.Adapters;

public class PublisherMongoDb(IMongoClient mongoClient, IOptions<MongoDbSettings> dbSettings) : IPublisherRepository
{
    private readonly IMongoCollection<Publisher> _collection =
            mongoClient
                .GetDatabase(dbSettings.Value.Database)
                .GetCollection<Publisher>(nameof(Publisher));

    public async Task<Publisher> Create(Publisher publisherPublisher, CancellationToken ct)
    {
        publisherPublisher.Id = Guid.NewGuid();
        publisherPublisher.CreatedAt = DateTime.UtcNow;
        await _collection
            .InsertOneAsync(publisherPublisher,
                cancellationToken: ct);

        return publisherPublisher;
    }

    public async Task Delete(Guid id, CancellationToken ct)
        => await _collection.DeleteOneAsync(x => x.Id == id, ct);

    public Task<Publisher> First(Expression<Func<Publisher, bool>> predicate, CancellationToken ct)
        => _collection.Find(predicate).FirstOrDefaultAsync(ct);

    public Task<long> Count(Expression<Func<Publisher, bool>> predicate, CancellationToken ct)
        => _collection.CountDocumentsAsync(predicate,  cancellationToken: ct);

    public async Task<Paginated<Publisher>> Get(Expression<Func<Publisher, bool>> filter, long skip, long take, CancellationToken ct)
    {
        var filtered = _collection.Find(filter);
        var total = await filtered.CountDocumentsAsync(ct);
        var results = await filtered.Skip((int)skip)
            .Limit((int)take)
            .SortBy(x => x.TradeName)
            .ThenBy(x => x.CompanyName)
            .ToListAsync(cancellationToken: ct);

        return new Paginated<Publisher>
        {
            Total = total,
            Results = results,
        };
    }


    public async Task<Publisher> Update(Publisher publisherPublisher, CancellationToken ct)
    {
        publisherPublisher.UpdatedAt = DateTime.UtcNow;
        await _collection
            .ReplaceOneAsync(
                x => x.Id == publisherPublisher.Id,
                publisherPublisher,
                cancellationToken: ct);

        return publisherPublisher;
    }

}