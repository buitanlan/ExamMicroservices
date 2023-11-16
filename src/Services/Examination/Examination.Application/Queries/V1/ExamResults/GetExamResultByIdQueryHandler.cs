using AutoMapper;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Queries.V1.ExamResults;

public class GetExamResultByIdQueryHandler(
    IExamResultRepository examResultRepository,
    IMapper mapper,
    ILogger<GetExamResultByIdQueryHandler> logger
    ) : IRequestHandler<GetExamResultByIdQuery, ApiResult<ExamResultDto>>
{
    public async Task<ApiResult<ExamResultDto>> Handle(GetExamResultByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("BEGIN: GetExamByIdQueryHandler");

        var result = await examResultRepository.GetDetails(request.Id);
        var item = mapper.Map<ExamResultDto>(result);

        logger.LogInformation("END: GetExamByIdQueryHandler");

        return new ApiSuccessResult<ExamResultDto>(200, item);
    }
}
