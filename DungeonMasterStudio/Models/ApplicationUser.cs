using Microsoft.AspNetCore.Identity;

namespace DungeonMasterStudio.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Connection> Connections { get; set; }
    }
}
