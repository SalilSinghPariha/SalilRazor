using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salil.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderHeaderForSessionPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionID",
                table: "orderHeaders",
                newName: "SessionId");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "orderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "orderHeaders");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "orderHeaders",
                newName: "TransactionID");
        }
    }
}
