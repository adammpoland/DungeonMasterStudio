using System.ComponentModel.DataAnnotations;

namespace DungeonMasterStudio.Models
{
    public class InventoryModel
    {
        [Key]
        public int InventoryID { get; set; }
        [Required]
        public int CharacterID { get; set; }
        public string ItemName { get; set; }
        public int Damage { get; set; }
        public bool IsEquiped { get; set; }

    }
}
