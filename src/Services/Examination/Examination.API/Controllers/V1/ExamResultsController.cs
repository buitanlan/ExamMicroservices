using System.Net;
using Examination.Application.Commands.V1.ExamResults.FinishExam;
using Examination.Application.Commands.V1.ExamResults.SkipExam;
using Examination.Application.Commands.V1.ExamResults.StartExam;
using Examination.Application.Commands.V1.ExamResults.SubmitQuestion;
using Examination.Application.Queries.V1.ExamResults;
using Examination.Application.Queries.V1.ExamResults.GetExamResultsByUserIdPaging;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V1;

    public class ExamResultsController(IMediator mediator, ILogger<ExamResultsController> logger) : BaseController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiSuccessResult<ExamResultDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetExamResultByIdAsync(string id)
        {
            logger.LogInformation("BEGIN: GetExamResultByIdAsync");

            var result = await mediator.Send(new GetExamResultByIdQuery(id));

            logger.LogInformation("END: GetExamResultByIdAsync");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("finish")]
        [ProducesResponseType(typeof(ApiSuccessResult<ExamResultDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FinishExamAsync(FinishExamRequest request)
        {
            logger.LogInformation("BEGIN: FinishExamAsync");

            var result = await mediator.Send(new FinishExamCommand() { ExamResultId = request.ExamResultId });

            logger.LogInformation("END: FinishExamAsync");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("skip")]
        [ProducesResponseType(typeof(ApiSuccessResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SkipExamAsync(SkipExamRequest request)
        {
            logger.LogInformation("BEGIN: SkipExamAsync");

            var result = await mediator.Send(new SkipExamCommand() { ExamResultId = request.ExamResultId });

            logger.LogInformation("END: SkipExamAsync");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("next-question")]
        [ProducesResponseType(typeof(ApiSuccessResult<ExamResultDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> NextQuestionAsync(NextQuestionRequest request)
        {
            logger.LogInformation("BEGIN: NextQuestionAsync");

            var result = await mediator.Send(new SubmitQuestionCommand()
            {
                ExamResultId = request.ExamResultId,
                QuestionId = request.QuestionId,
                AnswerIds = request.AnswerIds
            });

            logger.LogInformation("END: NextQuestionAsync");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("start")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> StartExamAsync([FromBody] StartExamRequest request)
        {
            logger.LogInformation("BEGIN: StartExamAsync");
            var result = await mediator.Send(new StartExamCommand()
            {
                ExamId = request.ExamId
            });

            logger.LogInformation("END: StartExamAsync");
            return StatusCode(result.StatusCode, result);
        }

        //api/v1/examResults/user/{id}
        [HttpGet("user")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamResultsByUserIdPagingAsync([FromQuery] GetExamResultsByUserIdPagingQuery request)
        {
            logger.LogInformation("BEGIN: GetExamResultsByUserIdPagingAsync");
            var result = await mediator.Send(request);

            logger.LogInformation("END: GetExamResultsByUserIdPagingAsync");
            return StatusCode(result.StatusCode, result);
        }
    }
