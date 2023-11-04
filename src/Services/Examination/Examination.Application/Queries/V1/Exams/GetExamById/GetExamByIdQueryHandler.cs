using AutoMapper;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Queries.V1.Exams.GetExamById;

public class GetExamByIdQueryHandler(
    IExamRepository examRepository,
    IMapper mapper,
    ILogger<GetExamByIdQueryHandler> logger)
    : IRequestHandler<GetExamByIdQuery, ApiResult<ExamDto>>
{
    public async Task<ApiResult<ExamDto>> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("BEGIN: GetExamByIdQueryHandler");

        var result = await examRepository.GetExamByIdAsync(request.Id);
        var item = mapper.Map<ExamDto>(result);

        logger.LogInformation("END: GetExamByIdQueryHandler");

        return new ApiSuccessResult<ExamDto>(200, item);
    }
}
