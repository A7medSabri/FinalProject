using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class addingChat2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_ClientId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_FreeLancerId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Chats",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "FreeLancerId",
                table: "Chats",
                newName: "ReceiverrId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_FreeLancerId",
                table: "Chats",
                newName: "IX_Chats_ReceiverrId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_ClientId",
                table: "Chats",
                newName: "IX_Chats_SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_ReceiverrId",
                table: "Chats",
                column: "ReceiverrId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_SenderId",
                table: "Chats",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_ReceiverrId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_SenderId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Chats",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "ReceiverrId",
                table: "Chats",
                newName: "FreeLancerId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_SenderId",
                table: "Chats",
                newName: "IX_Chats_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_ReceiverrId",
                table: "Chats",
                newName: "IX_Chats_FreeLancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_ClientId",
                table: "Chats",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_FreeLancerId",
                table: "Chats",
                column: "FreeLancerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
