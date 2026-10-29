using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
namespace PostsTags.Core.Entities
{
    public class AppUser :IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
