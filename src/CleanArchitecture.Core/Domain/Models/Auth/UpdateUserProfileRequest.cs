using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Domain.Models.Auth
{
    public class UpdateUserProfileRequest
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        public string? AvatarUrl { get; set; }

        public IFormFile? AvatarFile { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }
    }
}
