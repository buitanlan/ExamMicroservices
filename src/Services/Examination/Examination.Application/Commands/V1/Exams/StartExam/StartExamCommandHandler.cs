using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Commands.V1.Exams.StartExam;

public class StartExamCommandHandler(IExamResultRepository examResultRepository) : IRequestHandler<StartExamCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(StartExamCommand request, CancellationToken cancellationToken)
    {
        var examResult = await examResultRepository.GetDetails(request.UserId, request.ExamId);

        if (examResult != null)
        {
            examResult.ExamStartDate = DateTime.Now;
            examResult.Finished = false;
            examResult.StartExam(request.FirstName, request.LastName);

        }
        else
        {
            examResult = ExamResult.CreateNewResult(request.UserId, request.ExamId);
            examResult.StartExam(request.FirstName, request.LastName);
            await examResultRepository.InsertAsync(examResult);
        }
        await examResultRepository.CommitTransactionAsync(examResult, cancellationToken);
        return new ApiSuccessResult<bool>(200, true);    }
}
