using System.Net;
using Examination.Application.Commands.V1.Questions.CreateQuestion;
using Examination.Application.Commands.V1.Questions.DeleteQuestion;
using Examination.Application.Commands.V1.Questions.UpdateQuestion;
using Examination.Application.Queries.V1.Questions.GetQuestionById;
using Examination.Application.Queries.V1.Questions.GetQuestionsPaging;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Examination.API.Controllers.V1;

public class QuestionsController(IMediator mediator, ILogger<QuestionsController> logger) : BaseController
{
    private readonly ILogger<QuestionsController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<QuestionDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetQuestionsPagingAsync([FromQuery] GetQuestionsPagingQuery query)
    {
        Log.Information("BEGIN: GetQuestionsPagingAsync");

        var queryResult = await mediator.Send(query);

        Log.Information("END: GetQuestionsPagingAsync");

        return Ok(queryResult);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(QuestionDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetQuestionsByIdAsync(string id)
    {
        Log.Information("BEGIN: GetQuestionsByIdAsync");

        var queryResult = await mediator.Send(new GetQuestionByIdQuery(id));

        Log.Information("END: GetQuestionsByIdAsync");
        return Ok(queryResult);
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateQuestionAsync([FromBody] UpdateQuestionRequest request)
    {
        Log.Information("BEGIN: UpdateQuestionAsync");
        var queryResult = await mediator.Send(new UpdateQuestionCommand()
        {
            Id = request.Id,
            Content = request.Content,
            QuestionType = request.QuestionType,
            Level = request.Level,
            CategoryId = request.CategoryId,
            Answers = request.Answers,
            Explain = request.Explain
        });

        Log.Information("END: UpdateQuestionAsync");
        return Ok(queryResult);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateQuestionAsync([FromBody] CreateQuestionRequest request)
    {
        Log.Information("BEGIN: CreateQuestionAsync");

        var queryResult = await mediator.Send(new CreateQuestionCommand()
        {
            Content = request.Content,
            QuestionType = request.QuestionType,
            Level = request.Level,
            CategoryId = request.CategoryId,
            Answers = request.Answers,
            Explain = request.Explain
        });
        if (queryResult == null)
            return BadRequest();

        Log.Information("END: CreateQuestionAsync");
        return Ok(queryResult);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteQuestionAsync(string id)
    {
        Log.Information("BEGIN: GetExamList");

        var queryResult = await mediator.Send(new DeleteQuestionCommand(id));

        Log.Information("END: GetExamList");
        return Ok(queryResult);
    }
}

