using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoritesTable : Migration
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
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FavoriteJobPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FreelancerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobpostId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteJobPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteJobPost_AspNetUsers_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FavoriteJobPost_JobPosts_JobpostId",
                        column: x => x.JobpostId,
                        principalTable: "JobPosts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ClientId",
                table: "Favorites",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteJobPost_FreelancerId",
                table: "FavoriteJobPost",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteJobPost_JobpostId",
                table: "FavoriteJobPost",
                column: "JobpostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_ClientId",
                table: "Favorites",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_ClientId",
                table: "Favorites");

            migrationBuilder.DropTable(
                name: "FavoriteJobPost");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_ClientId",
                table: "Favorites");

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
