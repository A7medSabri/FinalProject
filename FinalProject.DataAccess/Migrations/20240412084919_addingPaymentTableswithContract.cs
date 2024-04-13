using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class addingPaymentTableswithContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Contracts",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PaymentMethodId",
                table: "Contracts",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_PaymentMethods_PaymentMethodId",
                table: "Contracts",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_PaymentMethods_PaymentMethodId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PaymentMethodId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Contracts");
        }
    }
}
