using AutoMapper;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Commands.V1.ExamResults.SubmitQuestion;

public class SubmitQuestionCommandHandler(IExamResultRepository examResultRepository, IMapper mapper)
    : IRequestHandler<SubmitQuestionCommand, ApiResult<ExamResultDto>>
{
    public async Task<ApiResult<ExamResultDto>> Handle(SubmitQuestionCommand request, CancellationToken cancellationToken)
    {
        var examResult = await examResultRepository.GetDetails(request.ExamResultId);

        var question = examResult.QuestionResults.FirstOrDefault(x => x.Id == request.QuestionId);
        if (question == null)
        {
            return new ApiResult<ExamResultDto>(400, false, "Cannot find the question");
        }
        foreach (var item in question.Answers)
        {
            item.UserChosen = request.AnswerIds?.Contains(item.Id);
        }
        await examResultRepository.UpdateAsync(examResult);
        var dto = mapper.Map<ExamResultDto>(examResult);
        return new ApiSuccessResult<ExamResultDto>(200, dto);
    }
}
