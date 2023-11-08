using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Core;

namespace PortalApp.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                AuthenticationConsts.SignInScheme,
                AuthenticationConsts.OidcAuthenticationScheme);
        }
    }
}
