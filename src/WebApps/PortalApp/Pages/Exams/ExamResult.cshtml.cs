using Examination.Shared.ExamResults;
using Examination.Shared.Exams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Exams;

public class ExamResultModel(IExamResultService examResultService, IExamService examService) : PageModel
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

    public string GetCharacterByIndex(int index)
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        var value = "";

        if (index >= letters.Length)
            value += letters[index / letters.Length - 1];

        value += letters[index % letters.Length];

        return value;
    }
}
