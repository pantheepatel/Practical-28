using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExceptionLogger.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }
    }
}
