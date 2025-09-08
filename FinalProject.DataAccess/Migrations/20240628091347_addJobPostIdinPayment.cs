using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class addJobPostIdinPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "jobId",
                table: "PaymentTests",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTests_jobId",
                table: "PaymentTests",
                column: "jobId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTests_JobPosts_jobId",
                table: "PaymentTests",
                column: "jobId",
                principalTable: "JobPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTests_JobPosts_jobId",
                table: "PaymentTests");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTests_jobId",
                table: "PaymentTests");

            migrationBuilder.DropColumn(
                name: "jobId",
                table: "PaymentTests");
        }
    }
}
