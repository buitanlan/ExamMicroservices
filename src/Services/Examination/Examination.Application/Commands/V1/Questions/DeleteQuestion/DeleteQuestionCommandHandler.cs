using Examination.Domain.AggregateModels.QuestionAggregate;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Questions.DeleteQuestion;

public class DeleteQuestionCommandHandler: IRequestHandler<DeleteQuestionCommand, bool>
{
    private readonly IQuestionRepository _questionRepository;

    public DeleteQuestionCommandHandler(
        IQuestionRepository QuestionRepository
    )
    {
        _questionRepository = QuestionRepository;

    }

    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await _questionRepository.GetQuestionsByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return false;
        }

        try
        {
            await _questionRepository.DeleteAsync(request.Id);
            return true;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex.Message);
            throw;
        }
    }
}
