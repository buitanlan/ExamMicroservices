using AutoMapper;
using Examination.Application.Extensions;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Examination.Application.Commands.V1.ExamResults.StartExam;

public class StartExamCommandHandler(
    IExamResultRepository examResultRepository,
    IExamRepository examRepository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper)
    : IRequestHandler<StartExamCommand, ApiResult<ExamResultDto>>
{
    public async Task<ApiResult<ExamResultDto>> Handle(StartExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await examRepository.GetExamByIdAsync(request.ExamId);
        var examResult = new ExamResult(httpContextAccessor.GetUserId(), request.ExamId);
        examResult.ExamStartDate = DateTime.UtcNow;
        examResult.ExamTitle = exam.Name;
        if (exam.IsTimeRestricted)
        {
            var durations = exam.Duration.Split(":");
            var durationTimeSpan = new TimeSpan(0, int.Parse(durations[0]), int.Parse(durations[1]));
            examResult.ExamFinishDate = DateTime.UtcNow.Add(durationTimeSpan);
        }

        examResult.CorrectQuestionCount = 0;
        examResult.QuestionResults = exam.Questions
            .Select(x => new QuestionResult(x.Id,
                x.Content,
                x.QuestionType,
                x.Level,
                x.Answers.Select(a => new AnswerResult(a.Id, a.Content, null, a.IsCorrect)).ToList(),
                x.Explain,
                null))
            .ToList(); ;
        examResult.Finished = false;

        await examResultRepository.InsertAsync(examResult);
        var dto = mapper.Map<ExamResultDto>(examResult);
        return new ApiSuccessResult<ExamResultDto>(200, dto);
    }
}
