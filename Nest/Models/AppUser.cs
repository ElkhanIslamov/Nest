using Microsoft.AspNetCore.Identity;

namespace Nest.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
    }
}
