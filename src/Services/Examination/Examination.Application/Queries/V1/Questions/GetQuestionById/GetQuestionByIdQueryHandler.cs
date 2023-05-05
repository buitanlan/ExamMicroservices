using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace Examination.Application.Queries.V1.Questions.GetQuestionById;

public class GetQuestionByIdQueryHandler(IQuestionRepository QuestionRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle)
    : IRequestHandler<GetQuestionByIdQuery, ApiResult<QuestionDto>>
{

    private readonly IQuestionRepository _questionRepository = QuestionRepository ?? throw new ArgumentNullException(nameof(QuestionRepository));
    private readonly IClientSessionHandle _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));

    public async Task<ApiResult<QuestionDto>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetQuestionByIdQueryHandler");

        var result = await _questionRepository.GetQuestionsByIdAsync(request.Id);
        var item = mapper.Map<QuestionDto>(result);

        Log.Information("END: GetQuestionByIdQueryHandler");

        return new ApiSuccessResult<QuestionDto>(item);
    }
}
