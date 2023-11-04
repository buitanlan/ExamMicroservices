using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Commands.V1.Exams.UpdateExam;

public class UpdateExamCommandHandler(
    IExamRepository examRepository,
    IQuestionRepository questionRepository,
    ICategoryRepository categoryRepository,
    ILogger<UpdateExamCommandHandler> logger,
    IMapper mapper)
    : IRequestHandler<UpdateExamCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await examRepository.GetExamByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            logger.LogError($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>(400, $"Item is not found {request.Id}");
        }

        if (request.NumberOfQuestions != itemToUpdate.NumberOfQuestions)
        {
            var questions = new List<Question>();
            if (request.AutoGenerateQuestion)
            {
                questions = await questionRepository.GetRandomQuestionsForExamAsync(request.CategoryId, request.Level, request.NumberOfQuestions);
            }
            else
            {
                questions = mapper.Map<List<QuestionDto>, List<Question>>(request.Questions);
            }
            itemToUpdate.Questions = questions;

        }

        if (itemToUpdate.Questions.Count < request.NumberOfQuestionCorrectForPass)
        {
            return new ApiErrorResult<bool>(400, "Number of questions is not engough for required");
        }


        if (request.CategoryId != itemToUpdate.CategoryId)
        {
            var category = await categoryRepository.GetCategoriesByIdAsync(request.CategoryId);
            itemToUpdate.CategoryId = category.Id;
            itemToUpdate.CategoryName = category.Name;
        }

        itemToUpdate.Name = request.Name;
        itemToUpdate.ShortDesc = request.ShortDesc;
        itemToUpdate.Content = request.Content;
        itemToUpdate.DurationInMinutes = request.DurationInMinutes;
        itemToUpdate.Level = request.Level;
        itemToUpdate.IsTimeRestricted = request.IsTimeRestricted;
        itemToUpdate.NumberOfQuestionCorrectForPass = request.NumberOfQuestionCorrectForPass;
        itemToUpdate.NumberOfQuestions = request.NumberOfQuestions;

        await examRepository.UpdateAsync(itemToUpdate);
        return new ApiSuccessResult<bool>(200, true, "Update successful");
    }
}
