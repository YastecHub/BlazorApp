using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlazorApp.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public ICollection<RefreshTokenModel> RefreshTokens { get; set; } = new List<RefreshTokenModel>();
    }
}
