﻿using System.ComponentModel.DataAnnotations;
using Examination.Shared.Enums;

namespace Examination.Shared.Questions;

public class UpdateQuestionRequest
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public QuestionType QuestionType { get; set; }

    [Required]
    public Level Level { set; get; }

    [Required]
    public string CategoryId { get; set; }

    [Required]
    public List<AnswerDto> Answers { set; get; } = new();

    public string Explain { get; set; }
}