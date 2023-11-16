using Examination.Shared.ExamResults;
using Examination.Shared.Exams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Exams;

[IgnoreAntiforgeryToken(Order = 1001)]
public class TakeExamModel(IExamResultService examResultService, IExamService examService)
    : PageModel
{
    [BindProperty]
    public ExamResultDto ExamResult { set; get; }

    [BindProperty]
    public ExamDto Exam { set; get; }

    public async Task OnGetAsync(string examResultId)
    {
        var result = await examResultService.GetExamResultByIdAsync(examResultId);
        if (result.IsSucceed)
        {
            ExamResult = result.ResultObj;

            var exam = await examService.GetExamByIdAsync(ExamResult.ExamId);
            if (exam.IsSucceed) { Exam = exam.ResultObj; }
        }
    }

    public async Task<IActionResult> OnGetQuestion(string examResultId, int questionIndex)
    {
        var examResult = await examResultService.GetExamResultByIdAsync(examResultId);
        if (examResult.IsSucceed)
        {
            return new JsonResult(examResult.ResultObj.QuestionResults[questionIndex]);
        }
        return BadRequest();
    }

    public async Task<IActionResult> OnPostSkipExamAsync([FromBody] SkipExamRequest request)
    {
        var exam = await examResultService.SkipExamAsync(request);
        if (exam.IsSucceed)
        {
            return new JsonResult(exam);
        }
        return BadRequest();
    }

    public async Task<IActionResult> OnPostFinishExamAsync([FromBody] FinishExamRequest request)
    {
        var exam = await examResultService.FinishExamAsync(request);
        if (exam.IsSucceed)
        {
            return new JsonResult(exam);
        }
        return BadRequest();
    }

    public async Task<IActionResult> OnPostNextQuestionAsync([FromBody] NextQuestionRequest request)
    {
        var exam = await examResultService.NextQuestionAsync(request);
        if (exam.IsSucceed)
        {
            return new JsonResult(exam);
        }
        return BadRequest();
    }
}
