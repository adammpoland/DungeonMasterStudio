using System.ComponentModel.DataAnnotations;

namespace DungeonMasterStudio.Models
{
    public class Party
    {
        [Key]
        public int PartyID { get; set; }
        [Required]
        public string UserID { get; set; }         //owner of the group
        [Required]
        public string Name { get; set; }
        public string Password { get; set; }
        public List<ApplicationUser> Members { get; set; } //owner will be included in Members as well
    }
}
