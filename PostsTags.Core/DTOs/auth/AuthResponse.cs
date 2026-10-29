using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostsTags.Core.DTOs.auth
{
     public class AuthResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
