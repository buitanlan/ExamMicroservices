using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Exams;

[Authorize]
public class ExamListModel(IExamService examService) : PageModel
{
    [BindProperty]
    public PagedList<ExamDto> Exam { set; get; }

    public async Task<IActionResult> OnGetAsync([FromQuery] ExamSearch search)
    {
        var result = await examService.GetExamsPagingAsync(search);
        if (result.IsSucceed)
        {
            Exam = result.ResultObj;
        }
        return Page();
    }
}
