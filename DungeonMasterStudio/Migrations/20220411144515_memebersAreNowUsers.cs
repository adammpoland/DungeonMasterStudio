using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonMasterStudio.Migrations
{
    public partial class memebersAreNowUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Parties_PartyID",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_PartyID",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PartyID",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "PartyID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PartyID",
                table: "AspNetUsers",
                column: "PartyID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Parties_PartyID",
                table: "AspNetUsers",
                column: "PartyID",
                principalTable: "Parties",
                principalColumn: "PartyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Parties_PartyID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PartyID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PartyID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PartyID",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_PartyID",
                table: "Members",
                column: "PartyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Parties_PartyID",
                table: "Members",
                column: "PartyID",
                principalTable: "Parties",
                principalColumn: "PartyID");
        }
    }
}
