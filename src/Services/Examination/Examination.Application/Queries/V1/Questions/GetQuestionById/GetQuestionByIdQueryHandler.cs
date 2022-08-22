using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace Examination.Application.Queries.V1.Questions.GetQuestionById;

public class GetQuestionByIdQueryHandler: IRequestHandler<GetQuestionByIdQuery, ApiResult<QuestionDto>>
{

    private readonly IQuestionRepository _questionRepository;
    private readonly IClientSessionHandle _clientSessionHandle;
    private readonly IMapper _mapper;

    public GetQuestionByIdQueryHandler(
        IQuestionRepository QuestionRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle
    )
    {
        _questionRepository = QuestionRepository ?? throw new ArgumentNullException(nameof(QuestionRepository));
        _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));
        _mapper = mapper;

    }

    public async Task<ApiResult<QuestionDto>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetQuestionByIdQueryHandler");

        var result = await _questionRepository.GetQuestionsByIdAsync(request.Id);
        var item = _mapper.Map<QuestionDto>(result);

        Log.Information("END: GetQuestionByIdQueryHandler");

        return new ApiSuccessResult<QuestionDto>(item);
    }
}
