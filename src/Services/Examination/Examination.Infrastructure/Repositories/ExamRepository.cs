﻿using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Infrastructure.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Examination.Infrastructure.Repositories;

public class ExamRepository(
        IMongoClient mongoClient,
        IClientSessionHandle clientSessionHandle,
        IOptions<ExamSettings> settings,
        IMediator mediator)
    : BaseRepository<Exam>(mongoClient, clientSessionHandle, settings, mediator, Constants.Collections.Exam),
        IExamRepository
{
    public async Task<Exam> GetExamByIdAsync(string id)
    {
        var filter = Builders<Exam>.Filter.Eq(s => s.Id, id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Exam>> GetExamListAsync()
    {
        return await Collection.AsQueryable().ToListAsync();
    }
}
