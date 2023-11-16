using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Profile
{
    public class ExamHistoriesModel(IExamResultService examResultService) : PageModel
    {
        [BindProperty]
        public PagedList<ExamResultDto> ExamResults { set; get; }

        public async Task OnGetAsync([FromQuery] PagingParameters request)
        {
            var result = await examResultService.GetExamResultsByUserIdPagingAsync(request);
            if (result.IsSucceed)
            {
                ExamResults = result.ResultObj;
            }
        }
    }
}
