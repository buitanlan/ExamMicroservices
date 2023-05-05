using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Questions.UpdateQuestion;

public class UpdateQuestionCommandHandler(IQuestionRepository questionRepository, IMapper mapper) : IRequestHandler<UpdateQuestionCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await questionRepository.GetQuestionsByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>($"Item is not found {request.Id}");
        }

        itemToUpdate.Content = request.Content;
        itemToUpdate.QuestionType = request.QuestionType;
        itemToUpdate.Level = request.Level;
        itemToUpdate.CategoryId = request.CategoryId;
        var answers = mapper.Map<List<AnswerDto>, List<Answer>>(request.Answers);
        itemToUpdate.Answers = answers;

        itemToUpdate.Explain = request.Explain;
        
        await questionRepository.UpdateAsync(itemToUpdate);
        return new ApiSuccessResult<bool>(true, "Delete successful");
    }
}