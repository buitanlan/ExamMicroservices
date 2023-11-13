using Examination.Shared.Exams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Exams;

public class ExamDetailsModel(IExamService examService) : PageModel
{
    [BindProperty]
    public ExamDto Exam { set; get; }

    public async Task<IActionResult> OnGet(string id)
    {
        var result = await examService.GetExamByIdAsync(id);
        if (!result.IsSucceed)
        {
            return NotFound();
        }

        Exam = result.ResultObj;
        return Page();
    }
}
