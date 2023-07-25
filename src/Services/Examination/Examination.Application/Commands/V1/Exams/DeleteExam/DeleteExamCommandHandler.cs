using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Commands.V1.Exams.DeleteExam;

public class DeleteExamCommandHandler(IExamRepository examRepository,ILogger logger)
    : IRequestHandler<DeleteExamCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await examRepository.GetExamByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            logger.LogError($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>(400, $"Item is not found {request.Id}");
        }

        await examRepository.DeleteAsync(request.Id);
        return new ApiSuccessResult<bool>(200, true, "Delete successful");
    }
}
