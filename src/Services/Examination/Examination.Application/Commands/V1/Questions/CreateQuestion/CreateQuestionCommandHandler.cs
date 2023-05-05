using AutoMapper;
using Examination.Application.Extensions;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Serilog;

namespace Examination.Application.Commands.V1.Questions.CreateQuestion;

public class CreateQuestionCommandHandler(IQuestionRepository questionRepository,
        IMapper mapper, ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<CreateQuestionCommand, ApiResult<QuestionDto>>
{
    public async Task<ApiResult<QuestionDto>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        if (request.Answers?.Count(x => x.IsCorrect) > 1 && request.QuestionType == Shared.Enums.QuestionType.SingleSelection)
        {
            return new ApiErrorResult<QuestionDto>("Single choice question cannot have multiple correct answers.");
        }
        var category = await categoryRepository.GetCategoriesByIdAsync(request.CategoryId);
        var questionId = ObjectId.GenerateNewId().ToString();
        foreach (var item in request.Answers)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                item.Id = ObjectId.GenerateNewId().ToString();
            }
        }
        var answers = mapper.Map<List<AnswerDto>, List<Answer>>(request.Answers);
        var itemToAdd = new Question(questionId,
            request.Content,
            request.QuestionType,
            request.Level,
            request.CategoryId,
            answers,
            request.Explain, 
            httpContextAccessor.GetUserId(), 
            category.Name);
    
        await questionRepository.InsertAsync(itemToAdd);
        var result = mapper.Map<Question, QuestionDto>(itemToAdd);
        return new ApiSuccessResult<QuestionDto>(result);
    }
}