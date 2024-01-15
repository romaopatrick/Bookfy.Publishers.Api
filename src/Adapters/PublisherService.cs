using Bookfy.Publishers.Api.Boundaries;
using Bookfy.Publishers.Api.Domain;
using Bookfy.Publishers.Api.Ports;

namespace Bookfy.Publishers.Api.Adapters;

public class PublisherService(IPublisherRepository repository) : IPublisherUseCase
{
    private readonly IPublisherRepository _repository = repository;
    public async Task<Result<Publisher>> Create(CreatePublisher input, CancellationToken ct)
    {
        if (!input.Document.ValidCnpj())
            return Result.WithFailure<Publisher>("invalid_document", 400);

        if (await PublisherConflicts(input.CompanyName, input.TradeName, input.Document, ct))
            return Result.WithFailure<Publisher>("publisher_conflict", 409);

        var publisher = await _repository
            .Create(new()
            {
                CompanyName = input.CompanyName.ToLower(),
                Document = DocumentExtensions.ClearDocument(input.Document),
                Settings = input.Settings,
                TradeName = input.TradeName.ToLower(),
            }, ct);

        return Result.WithSuccess(publisher, 201);
    }

    public async Task<Result> Delete(DeletePublisher input, CancellationToken ct)
    {
        var publisher = await _repository.First(x => x.Id == input.Id, ct);
        if (publisher == null)
            return Result.WithFailure<Publisher>("publisher_not_found_with_id", 404);

        await _repository.Delete(publisher.Id, ct);

        return Result.WithSuccess(publisher, 204);
    }


    public async Task<Result<Paginated<Publisher>>> Get(SearchPublishers input, CancellationToken ct)
    {
        var result = await _repository.Get(x
            => x.CompanyName.StartsWith(input.SearchTerm ?? "", StringComparison.CurrentCultureIgnoreCase)
            || x.TradeName.StartsWith(input.SearchTerm ?? "", StringComparison.CurrentCultureIgnoreCase)
            || x.Document.Contains(
                DocumentExtensions.ClearDocument(input.SearchTerm ?? ""), StringComparison.CurrentCultureIgnoreCase),
            input.Skip ?? 0, input.Take ?? 10, ct);

        return Result.WithSuccess(result, 200);
    }

    public async Task<Result<Publisher>> GetById(GetPublisherById input, CancellationToken ct)
    {
        var publisher = await _repository.First(x => x.Id == input.Id, ct);
        return publisher is null
            ? Result.WithFailure<Publisher>("publisher_not_found_with_id", 404)
            : Result.WithSuccess(publisher, 200); 
    }

    public async Task<Result<Publisher>> Update(UpdatePublisher input, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(input.Document) && !input.Document!.ValidCnpj())
            return Result.WithFailure<Publisher>("invalid_document", 400);

        var publisher = await _repository.First(x => x.Id == input.Id, ct);
        if (publisher is null)
            return Result.WithFailure<Publisher>("publisher_not_found_with_id", 404);

        if (await PublisherConflicts(
                input.Id,
                input.CompanyName ?? publisher.CompanyName,
                input.TradeName ?? publisher.TradeName,
                input.Document ?? publisher.Document,
                ct))
            return Result.WithFailure<Publisher>("publisher_conflict", 409);

        publisher.TradeName = input.TradeName ?? publisher.TradeName;
        publisher.CompanyName = input.CompanyName ?? publisher.CompanyName;
        publisher.Document = input.Document ?? publisher.Document;

        publisher = await _repository.Update(publisher, ct);

        return Result.WithSuccess(publisher, 200);
    }

    private async Task<bool> PublisherConflicts(string companyName, string tradeName, string document, CancellationToken ct)
    {
        var count = await _repository.Count(x
            => x.CompanyName.Equals(companyName, StringComparison.CurrentCultureIgnoreCase)
            || x.TradeName.Equals(tradeName, StringComparison.CurrentCultureIgnoreCase)
            || x.Document.Equals(document, StringComparison.CurrentCultureIgnoreCase),
            ct);

        return count > 0;
    }
    private async Task<bool> PublisherConflicts(Guid exceptId, string companyName, string tradeName, string document, CancellationToken ct)
    {
        var count = await _repository.Count(x
            => x.Id != exceptId
            && (x.CompanyName.Equals(companyName, StringComparison.CurrentCultureIgnoreCase)
                || x.TradeName.Equals(tradeName, StringComparison.CurrentCultureIgnoreCase)
                || x.Document.Equals(document, StringComparison.CurrentCultureIgnoreCase)
            ),
            ct);

        return count > 0;
    }
}