using Microsoft.AspNetCore.Identity;

namespace QLThuvien.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; } = string.Empty;
    }
}