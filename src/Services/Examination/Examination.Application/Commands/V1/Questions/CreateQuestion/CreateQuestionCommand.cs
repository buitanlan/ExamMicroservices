﻿using System.ComponentModel.DataAnnotations;
using Examination.Shared.Enums;
using Examination.Shared.Questions;
using MediatR;

namespace Examination.Application.Commands.V1.Questions.CreateQuestion;

public class CreateQuestionCommand: IRequest<QuestionDto>
{
    [Required]
    public string Content { get; set; }

    [Required]
    public QuestionType QuestionType { get; set; }

    [Required]
    public Level Level { set; get; }

    [Required]
    public string CategoryId { get; set; }

    [Required]
    public List<AnswerDto> Answers { set; get; } = new ();

    public string Explain { get; set; }
}