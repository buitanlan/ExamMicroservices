using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Questions.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, ApiResult<bool>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;

    }

    public async Task<ApiResult<bool>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await _questionRepository.GetQuestionsByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>($"Item is not found {request.Id}");
        }

        itemToUpdate.Content = request.Content;
        itemToUpdate.QuestionType = request.QuestionType;
        itemToUpdate.Level = request.Level;
        itemToUpdate.CategoryId = request.CategoryId;
        var answers = _mapper.Map<List<AnswerDto>, List<Answer>>(request.Answers);
        itemToUpdate.Answers = answers;

        itemToUpdate.Explain = request.Explain;
        
        await _questionRepository.UpdateAsync(itemToUpdate);
        return new ApiSuccessResult<bool>(true, "Delete successful");
    }
}