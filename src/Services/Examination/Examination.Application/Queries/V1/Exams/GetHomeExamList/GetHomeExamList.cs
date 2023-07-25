﻿using Examination.Shared.Exams;
using MediatR;

namespace Examination.Application.Queries.V1.Exams.GetHomeExamList;

public class GetHomeExamListQuery : IRequest<IEnumerable<ExamDto>>;