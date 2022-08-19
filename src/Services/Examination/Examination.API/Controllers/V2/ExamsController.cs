using Examination.Application.Queries.V1.Exams.GetHomeExamList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V2;
[ApiVersion("2.0")]
public class ExamsController : BaseController
{
    private readonly IMediator _mediator;

    public ExamsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetExamList(string sample)
    {
        var query = new GetHomeExamListQuery();
        var queryResult = await _mediator.Send(query);
        return Ok(queryResult);
    }

}