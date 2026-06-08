using Microsoft.AspNetCore.Identity;

namespace WritingSessionsAspApi.Models;

public class AppUser : IdentityUser
{
    public DateTime SignUpDate { get; set; } = DateTime.Today;
}