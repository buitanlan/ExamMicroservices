using Examination.Shared.Enums;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Commands.V1.Questions.UpdateQuestion;

public class UpdateQuestionCommand: IRequest<ApiResult<bool>>
{
    public string Id { get; set; }

    public string Content { get; set; }


    public QuestionType QuestionType { get; set; }


    public Level Level { set; get; }


    public string CategoryId { get; set; }


    public List<AnswerDto> Answers { set; get; } = new();

    public string Explain { get; set; }
}