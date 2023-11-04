using Examination.Application.Queries.V1.Exams.GetAllExams;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V2;
[ApiVersion("2.0")]
public class ExamsController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetExamList(string sample)
    {
        var query = new GetAllExamsQuery();
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

}
