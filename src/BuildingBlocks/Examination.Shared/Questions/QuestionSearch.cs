﻿using Examination.Shared.SeedWork;

namespace Examination.Shared.Questions;

public class QuestionSearch: PagingParameters
{
    public string CategoryId { get; set; }
    public string Name { get; set; }

}