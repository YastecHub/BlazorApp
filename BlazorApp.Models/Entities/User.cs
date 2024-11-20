using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlazorApp.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public ICollection<RefreshTokenModel> RefreshTokens { get; set; } = new List<RefreshTokenModel>();
    }
}
