using Examination.Application.Queries.V1.Exams.GetHomeExamList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V2;
[ApiVersion("2.0")]
public class ExamsController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetExamList(string sample)
    {
        var query = new GetHomeExamListQuery();
        var queryResult = await mediator.Send(query);
        return Ok(queryResult);
    }

}