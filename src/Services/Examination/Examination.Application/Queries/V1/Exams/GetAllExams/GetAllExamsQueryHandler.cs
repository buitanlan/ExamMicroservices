using AutoMapper;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Queries.V1.Exams.GetAllExams;

public class GetAllExamsQueryHandler(IExamRepository examRepository, IMapper mapper)
    : IRequestHandler<GetAllExamsQuery, ApiResult<IEnumerable<ExamDto>>>
{
    public async Task<ApiResult<IEnumerable<ExamDto>>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
    {
        var exams = await examRepository.GetAllExamsAsync();
        var examDtos = mapper.Map<IEnumerable<ExamDto>>(exams);
        return new ApiSuccessResult<IEnumerable<ExamDto>>(200, examDtos);
    }
}
