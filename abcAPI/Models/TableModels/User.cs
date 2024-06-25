using Microsoft.AspNetCore.Identity;

namespace abcAPI.Models;

public class User : IdentityUser
{
    public string Nickname { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}