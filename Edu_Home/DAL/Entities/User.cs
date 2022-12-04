using Microsoft.AspNetCore.Identity;

namespace Edu_Home.DAL.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get;set; }
        public string? LastName { get;set; }
    }
}
