using Microsoft.AspNetCore.Identity;

namespace IdentityServer.API.Models;

public class ApplicationUser: IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}