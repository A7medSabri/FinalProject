using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class EditFavoritesFreelanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_JobPosts_JobpostId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_JobpostId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "JobpostId",
                table: "Favorites");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Favorites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Favorites");

            migrationBuilder.AddColumn<int>(
                name: "JobpostId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_JobpostId",
                table: "Favorites",
                column: "JobpostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_JobPosts_JobpostId",
                table: "Favorites",
                column: "JobpostId",
                principalTable: "JobPosts",
                principalColumn: "Id");
        }
    }
}
