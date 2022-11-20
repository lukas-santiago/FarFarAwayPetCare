using Microsoft.AspNetCore.Identity;

namespace Application.Models;

public class UserToken
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}

public class ApplicationUser : IdentityUser
{ }