using Examination.Application.Queries.V1.Exams.GetHomeExamList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V1;

public class ExamsController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetExamList()
    {
        var query = new GetHomeExamListQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

}