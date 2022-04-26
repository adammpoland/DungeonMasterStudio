using DungeonMasterStudio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DungeonMasterStudio.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CharacterModel> Characters { get; set; }
        public DbSet<InventoryModel> InventoryItems { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }   
        public DbSet<Member> Members { get; set; }   
    }
}