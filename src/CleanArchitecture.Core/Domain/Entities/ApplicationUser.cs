using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Core.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public DateTime? LastSignInDate { get; set; }
    }
}
