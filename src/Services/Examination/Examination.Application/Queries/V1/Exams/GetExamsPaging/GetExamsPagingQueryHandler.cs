using AutoMapper;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Queries.V1.Exams.GetExamsPaging;

public class GetExamsPagingQueryHandler(
    IExamRepository examRepository,
    IMapper mapper,
    ILogger<GetExamsPagingQueryHandler> logger)
    : IRequestHandler<GetExamsPagingQuery, ApiResult<PagedList<ExamDto>>>
{
    public async Task<ApiResult<PagedList<ExamDto>>> Handle(GetExamsPagingQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("BEGIN: GetExamsPagingQueryHandler");

        var result = await examRepository.GetExamsPagingAsync(request.CategoryId,
            request.SearchKeyword,
            request.PageIndex,
            request.PageSize);

        var items = mapper.Map<List<ExamDto>>(result.Items);

        logger.LogInformation("END: GetExamsPagingQueryHandler");
        var pagedItems = new PagedList<ExamDto>(items, result.MetaData.TotalCount, request.PageIndex, request.PageSize);

        return new ApiSuccessResult<PagedList<ExamDto>>(200, pagedItems);
    }
}
