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

    private readonly IQuestionRepository _QuestionRepository;
    private readonly IClientSessionHandle _clientSessionHandle;
    private readonly IMapper _mapper;

    public GetQuestionsPagingQueryHandler(
        IQuestionRepository QuestionRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle
    )
    {
        _QuestionRepository = QuestionRepository ?? throw new ArgumentNullException(nameof(QuestionRepository));
        _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));
        _mapper = mapper;

    }

    public async Task<PagedList<QuestionDto>> Handle(GetQuestionsPagingQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetHomeExamListQueryHandler");

        var result = await _QuestionRepository.GetQuestionsPagingAsync(request.SearchKeyword, request.PageIndex, request.PageSize);
        var items = _mapper.Map<List<QuestionDto>>(result.Item1);

        Log.Information("END: GetHomeExamListQueryHandler");
        return new PagedList<QuestionDto>(items, result.Item2, request.PageIndex, request.PageSize);
    }
}