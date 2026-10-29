using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostsTags.Core.DTOs.auth
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } =string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } =string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } =string.Empty;
    }
}
