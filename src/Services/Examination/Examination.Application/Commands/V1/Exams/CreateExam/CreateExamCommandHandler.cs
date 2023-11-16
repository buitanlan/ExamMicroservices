using System.Net;
using AutoMapper;
using Examination.Application.Extensions;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Exams;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Commands.V1.Exams.CreateExam;

public class CreateExamCommandHandler(
    IExamRepository examRepository,
    IQuestionRepository questionRepository,
    ILogger<CreateExamCommandHandler> logger,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor,
    ICategoryRepository categoryRepository)
    : IRequestHandler<CreateExamCommand, ApiResult<ExamDto>>
{
    public async Task<ApiResult<ExamDto>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
    {
        List<Question> questions;
        if (request.AutoGenerateQuestion)
        {
            questions = await questionRepository.GetRandomQuestionsForExamAsync(request.CategoryId, request.Level,
                request.NumberOfQuestions);
        }
        else
        {
            questions = mapper.Map<List<QuestionDto>, List<Question>>(request.Questions);
        }

        var category = await categoryRepository.GetCategoriesByIdAsync(request.CategoryId);
        var currentUserId = httpContextAccessor.GetUserId();
        var itemToAdd = new Exam(
            request.Name,
            request.ShortDesc,
            request.Content,
            request.NumberOfQuestions,
            request.Duration,
            questions,
            request.Level,
            currentUserId,
            request.NumberOfQuestionCorrectForPass,
            request.IsTimeRestricted,
            request.CategoryId,
            category.Name);

        await examRepository.InsertAsync(itemToAdd);

        var result = mapper.Map<Exam, ExamDto>(itemToAdd);

        return new ApiSuccessResult<ExamDto>(HttpStatusCode.OK, result);
    }
}
