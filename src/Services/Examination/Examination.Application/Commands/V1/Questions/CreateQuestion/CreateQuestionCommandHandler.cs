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

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, ApiResult<QuestionDto>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CreateQuestionCommandHandler(
        IQuestionRepository questionRepository,
        IMapper mapper, ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResult<QuestionDto>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        if (request.Answers?.Count(x => x.IsCorrect) > 1 && request.QuestionType == Shared.Enums.QuestionType.SingleSelection)
        {
            return new ApiErrorResult<QuestionDto>("Single choice question cannot have multiple correct answers.");
        }
        var category = await _categoryRepository.GetCategoriesByIdAsync(request.CategoryId);
        var questionId = ObjectId.GenerateNewId().ToString();
        foreach (var item in request.Answers)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                item.Id = ObjectId.GenerateNewId().ToString();
            }
        }
        var answers = _mapper.Map<List<AnswerDto>, List<Answer>>(request.Answers);
        var itemToAdd = new Question(questionId,
            request.Content,
            request.QuestionType,
            request.Level,
            request.CategoryId,
            answers,
            request.Explain, 
            _httpContextAccessor.GetUserId(), 
            category.Name);
    
        await _questionRepository.InsertAsync(itemToAdd);
        var result = _mapper.Map<Question, QuestionDto>(itemToAdd);
        return new ApiSuccessResult<QuestionDto>(result);
    }
}