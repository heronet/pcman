using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pcman.Models;

namespace pcman.Data
{
    public class ApplicationDbContext : IdentityDbContext<EntityUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Computer> Computers { get; set; }
    }
}