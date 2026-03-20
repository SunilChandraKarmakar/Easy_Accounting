using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurchaseTable011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Purchase",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_CompanyId",
                schema: "Purchase",
                table: "Purchases",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Companies_CompanyId",
                schema: "Purchase",
                table: "Purchases",
                column: "CompanyId",
                principalSchema: "MasterSettings",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Companies_CompanyId",
                schema: "Purchase",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_CompanyId",
                schema: "Purchase",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Purchase",
                table: "Purchases");
        }
    }
}
