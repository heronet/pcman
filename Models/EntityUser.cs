using Microsoft.AspNetCore.Identity;

namespace pcman.Models
{
    public class EntityUser : IdentityUser
    {
        public string Name { get; set; }
        public string EduId { get; set; }
    }
}