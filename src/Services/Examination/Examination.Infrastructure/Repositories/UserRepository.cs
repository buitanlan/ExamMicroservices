using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Infrastructure.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositories;

public class UserRepository(
        IMongoClient mongoClient,
        IClientSessionHandle clientSessionHandle,
        IOptions<ExamSettings> settings,
        IMediator mediator)
    : BaseRepository<User>(mongoClient, clientSessionHandle, settings, mediator, Constants.Collections.User),
        IUserRepository
{
    public async Task<User> GetUserByIdAsync(string externalId)
    {
        var filter = Builders<User>.Filter.Eq(s => s.ExternalId, externalId);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
}
