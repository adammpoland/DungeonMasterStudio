using System.ComponentModel.DataAnnotations;

namespace DungeonMasterStudio.Models
{
    public class Member
    {
        //need to add a different key so that different users can join multiple parties
        [Key]
        public string UserId { get; set; } //user 
        public DateTime DateAdded { get; set; } = DateTime.Now;
        
    }
}
