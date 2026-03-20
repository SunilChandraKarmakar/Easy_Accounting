using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurchaseTable012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                schema: "Purchase",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                schema: "Purchase",
                table: "Purchases");
        }
    }
}
