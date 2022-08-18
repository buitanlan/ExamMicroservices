using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Questions.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, bool>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;

    }

    public async Task<bool> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await _questionRepository.GetQuestionsByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return false;
        }

        itemToUpdate.Content = request.Content;
        itemToUpdate.QuestionType = request.QuestionType;
        itemToUpdate.Level = request.Level;
        itemToUpdate.CategoryId = request.CategoryId;
        var answers = _mapper.Map<List<AnswerDto>, List<Answer>>(request.Answers);
        itemToUpdate.Answers = answers;

        itemToUpdate.Explain = request.Explain;


        try
        {
            await _questionRepository.UpdateAsync(itemToUpdate);
        }
        catch (Exception ex)
        {

            Log.Fatal(ex.Message);
            throw;
        }

        return true;
    }
}