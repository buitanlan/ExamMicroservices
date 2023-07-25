using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace Examination.Application.Queries.V1.Questions.GetQuestionsPaging;

public class GetQuestionsPagingQueryHandler(IQuestionRepository questionRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle)
    : IRequestHandler<GetQuestionsPagingQuery, ApiResult<PagedList<QuestionDto>>>
{

    private readonly IQuestionRepository _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
    private readonly IClientSessionHandle _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));

    public async Task<ApiResult<PagedList<QuestionDto>>> Handle(GetQuestionsPagingQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetHomeExamListQueryHandler");

        var result = await _questionRepository.GetQuestionsPagingAsync(request.CategoryId, 
            request.SearchKeyword,
            request.PageIndex,
            request.PageSize);
        var items = mapper.Map<List<QuestionDto>>(result.Items);

        var pagedItems = new PagedList<QuestionDto>(items, result.MetaData.TotalCount, request.PageIndex, request.PageSize);

        Log.Information("END: GetHomeExamListQueryHandler");
        return new ApiSuccessResult<PagedList<QuestionDto>>(pagedItems);
    }
}