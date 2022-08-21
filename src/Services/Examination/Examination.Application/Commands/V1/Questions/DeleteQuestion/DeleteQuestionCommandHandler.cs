using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Questions.DeleteQuestion;

public class DeleteQuestionCommandHandler: IRequestHandler<DeleteQuestionCommand, ApiResult<bool>>
{
    private readonly IQuestionRepository _questionRepository;

    public DeleteQuestionCommandHandler(
        IQuestionRepository QuestionRepository
    )
    {
        _questionRepository = QuestionRepository;

    }

    public async Task<ApiResult<bool>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await _questionRepository.GetQuestionsByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>($"Item is not found {request.Id}");
        }

        await _questionRepository.DeleteAsync(request.Id);
        return new ApiSuccessResult<bool>(true, "Delete successful");
   
    }
}
