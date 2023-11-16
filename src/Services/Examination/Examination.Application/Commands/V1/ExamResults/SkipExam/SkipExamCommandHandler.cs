using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Commands.V1.ExamResults.SkipExam;

public class SkipExamCommandHandler(IExamResultRepository examResultRepository) : IRequestHandler<SkipExamCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(SkipExamCommand request, CancellationToken cancellationToken)
    {
        var examResult = await examResultRepository.GetDetails(request.ExamResultId);
        if (examResult == null)
            return new ApiResult<bool>(400, false, "Cannot found");

        await examResultRepository.DeleteAsync(request.ExamResultId);
        return new ApiSuccessResult<bool>(200, true);
    }
}
