using System.Net;
using Examination.Application.Commands.V1.Exams.CreateExam;
using Examination.Application.Commands.V1.Exams.DeleteExam;
using Examination.Application.Commands.V1.Exams.UpdateExam;
using Examination.Application.Queries.V1.Exams.GetAllExams;
using Examination.Application.Queries.V1.Exams.GetExamById;
using Examination.Application.Queries.V1.Exams.GetExamsPaging;
using Examination.Shared.Enums;
using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V1;

public class ExamsController(IMediator mediator, ILogger<ExamsController> logger) : BaseController
{
        [HttpGet]
        public async Task<IActionResult> GetAllExams()
        {
            logger.LogInformation("BEGIN: GetAllExams");

            var result = await mediator.Send(new GetAllExamsQuery());

            logger.LogInformation("END: GetAllExams");
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("paging")]
        [ProducesResponseType(typeof(ApiSuccessResult<PagedList<ExamDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamsPagingAsync([FromQuery] GetExamsPagingQuery query)
        {
            logger.LogInformation("BEGIN: GetExamsPagingAsync");

            var result = await mediator.Send(query);

            logger.LogInformation("END: GetExamsPagingAsync");

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiSuccessResult<ExamDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetExamByIdAsync(string id)
        {
            logger.LogInformation("BEGIN: GetExamByIdAsync");

            var result = await mediator.Send(new GetExamByIdQuery(id));

            logger.LogInformation("END: GetExamByIdAsync");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateExamAsync([FromBody] UpdateExamRequest request)
        {
            logger.LogInformation("BEGIN: UpdateExamAsync");
            var result = await mediator.Send(new UpdateExamCommand()
            {
                Id = request.Id,
                Name = request.Name,
                AutoGenerateQuestion = request.AutoGenerateQuestion,
                CategoryId = request.CategoryId,
                Content = request.Content,
                DurationInMinutes = request.DurationInMinutes,
                IsTimeRestricted = request.IsTimeRestricted,
                Level = request.Level,
                NumberOfQuestionCorrectForPass = request.NumberOfQuestionCorrectForPass,
                NumberOfQuestions = request.NumberOfQuestions,
                Questions = request.Questions,
                ShortDesc = request.ShortDesc
            });

            logger.LogInformation("END: UpdateExamAsync");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateExamAsync([FromBody] CreateExamRequest request)
        {
            logger.LogInformation("BEGIN: CreateExamAsync");

            var result = await mediator.Send(new CreateExamCommand()
            {
                Name = request.Name,
                AutoGenerateQuestion = request.AutoGenerateQuestion,
                CategoryId = request.CategoryId,
                Content = request.Content,
                DurationInMinutes = request.DurationInMinutes,
                IsTimeRestricted = request.IsTimeRestricted,
                Level = request.Level,
                NumberOfQuestionCorrectForPass = request.NumberOfQuestionCorrectForPass,
                NumberOfQuestions = request.NumberOfQuestions,
                Questions = request.Questions,
                ShortDesc = request.ShortDesc
            });
            logger.LogInformation("END: CreateExamAsync");
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteExamAsync(string id)
        {
            logger.LogInformation("BEGIN: DeleteExamAsync");

            var result = await mediator.Send(new DeleteExamCommand(id));

            logger.LogInformation("END: DeleteExamAsync");
            return StatusCode(result.StatusCode, result);
        }

}
