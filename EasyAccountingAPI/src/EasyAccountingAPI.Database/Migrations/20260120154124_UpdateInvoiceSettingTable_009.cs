using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceSettingTable_009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "MasterSettings",
                table: "InvoiceSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSettings_CompanyId",
                schema: "MasterSettings",
                table: "InvoiceSettings",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceSettings_Companies_CompanyId",
                schema: "MasterSettings",
                table: "InvoiceSettings",
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
                name: "FK_InvoiceSettings_Companies_CompanyId",
                schema: "MasterSettings",
                table: "InvoiceSettings");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceSettings_CompanyId",
                schema: "MasterSettings",
                table: "InvoiceSettings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "MasterSettings",
                table: "InvoiceSettings");
        }
    }
}
