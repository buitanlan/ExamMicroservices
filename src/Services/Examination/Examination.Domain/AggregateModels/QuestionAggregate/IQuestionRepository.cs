using Examination.Domain.SeedWord;

namespace Examination.Domain.AggregateModels.QuestionAggregate;

public interface IQuestionRepository: IRepositoryBase<Question>
{
    Task<Tuple<List<Question>, long>> GetQuestionsPagingAsync(string searchKeyword, int pageIndex, int pageSize);

    Task<Question> GetQuestionsByIdAsync(string id);
}