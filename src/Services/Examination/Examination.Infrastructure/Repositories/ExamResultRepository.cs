using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Infrastructure.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositories;

public class ExamResultRepository(
        IMongoClient mongoClient,
        IClientSessionHandle clientSessionHandle,
        IOptions<ExamSettings> settings,
        IMediator mediator)
    : BaseRepository<ExamResult>(mongoClient, clientSessionHandle, settings, mediator,
        Constants.Collections.ExamResult), IExamResultRepository
{
    public async Task<ExamResult> GetDetails(string id)
    {
        var filter = Builders<ExamResult>.Filter.Where(s => s.Id == id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
}
