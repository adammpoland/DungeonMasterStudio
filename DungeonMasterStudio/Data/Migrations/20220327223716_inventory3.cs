using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonMasterStudio.Data.Migrations
{
    public partial class inventory3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CharachterID",
                table: "InventoryItems",
                newName: "CharacterID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CharacterID",
                table: "InventoryItems",
                newName: "CharachterID");
        }
    }
}
