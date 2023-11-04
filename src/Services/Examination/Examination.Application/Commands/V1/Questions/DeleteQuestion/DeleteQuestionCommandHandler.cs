using System.Net;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Questions.DeleteQuestion;

public class DeleteQuestionCommandHandler(IQuestionRepository QuestionRepository) : IRequestHandler<DeleteQuestionCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await QuestionRepository.GetQuestionsByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>(HttpStatusCode.BadRequest,$"Item is not found {request.Id}");
        }

        await QuestionRepository.DeleteAsync(request.Id);
        return new ApiSuccessResult<bool>(HttpStatusCode.OK,true, "Delete successful");
   
    }
}
