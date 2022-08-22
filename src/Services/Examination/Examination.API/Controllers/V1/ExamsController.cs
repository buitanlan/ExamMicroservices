﻿using Examination.Application.Queries.V1.Exams.GetHomeExamList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V1;

public class ExamsController : BaseController
{
    private readonly IMediator _mediator;

    public ExamsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetExamList()
    {
        var query = new GetHomeExamListQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}