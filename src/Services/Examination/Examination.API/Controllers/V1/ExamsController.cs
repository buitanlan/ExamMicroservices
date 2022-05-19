using Examination.Application.Queries.V1.GetHomeExamList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V1;

[ApiVersion("1.0")]
[Authorize]
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
        var queryResult = await _mediator.Send(query);
        return Ok(queryResult);
    }

}