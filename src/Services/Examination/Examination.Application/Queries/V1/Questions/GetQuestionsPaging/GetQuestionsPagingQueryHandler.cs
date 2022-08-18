using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace Examination.Application.Queries.V1.Questions.GetQuestionsPaging;

public class GetQuestionsPagingQueryHandler: IRequestHandler<GetQuestionsPagingQuery, PagedList<QuestionDto>>
{

    private readonly IQuestionRepository _questionRepository;
    private readonly IClientSessionHandle _clientSessionHandle;
    private readonly IMapper _mapper;

    public GetQuestionsPagingQueryHandler(
        IQuestionRepository questionRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle
    )
    {
        _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
        _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));
        _mapper = mapper;

    }

    public async Task<PagedList<QuestionDto>> Handle(GetQuestionsPagingQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetHomeExamListQueryHandler");

        var result = await _questionRepository.GetQuestionsPagingAsync(request.CategoryId, 
            request.SearchKeyword,
            request.PageIndex,
            request.PageSize);

        var items = _mapper.Map<List<QuestionDto>>(result.Items);

        Log.Information("END: GetHomeExamListQueryHandler");
        return new PagedList<QuestionDto>(items, result.MetaData.TotalCount, request.PageIndex, request.PageSize);
    }
}