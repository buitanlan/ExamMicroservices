using Examination.Domain.SeedWord;

namespace Examination.Domain.AggregateModels.ExamResultAggregate;

public interface IExamResultRepository : IRepositoryBase<ExamResult>
{
    Task<ExamResult> GetDetails(string userId, string examId);
}