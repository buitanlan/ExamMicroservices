using Examination.Domain.SeedWord;
using Examination.Shared.SeedWork;

namespace Examination.Domain.AggregateModels.ExamResultAggregate;

public interface IExamResultRepository : IRepositoryBase<ExamResult>
{
    Task<ExamResult> GetDetails(string id);

    Task<PagedList<ExamResult>> GetExamResultsByUserIdPagingAsync(string userId, int pageIndex, int pageSize);

}
