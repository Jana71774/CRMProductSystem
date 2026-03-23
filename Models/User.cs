using System.ComponentModel.DataAnnotations;

namespace CRMProductSystem.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? Role { get; set; } // Admin / Employee
    }
}