using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonMasterStudio.Data.Migrations
{
    public partial class characters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArmorClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentHitPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxHitPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dexterity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Constitution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Intelligence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wisdom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Charisma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackStory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
