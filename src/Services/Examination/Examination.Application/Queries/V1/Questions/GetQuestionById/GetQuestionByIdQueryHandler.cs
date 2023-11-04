using System.Net;
using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using Serilog;

namespace Examination.Application.Queries.V1.Questions.GetQuestionById;

public class GetQuestionByIdQueryHandler(IQuestionRepository questionRepository, IMapper mapper)
    : IRequestHandler<GetQuestionByIdQuery, ApiResult<QuestionDto>>
{
    public async Task<ApiResult<QuestionDto>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetQuestionByIdQueryHandler");

        var result = await questionRepository.GetQuestionsByIdAsync(request.Id);
        var item = mapper.Map<QuestionDto>(result);

        Log.Information("END: GetQuestionByIdQueryHandler");

        return new ApiSuccessResult<QuestionDto>(HttpStatusCode.OK, item);
    }
}
