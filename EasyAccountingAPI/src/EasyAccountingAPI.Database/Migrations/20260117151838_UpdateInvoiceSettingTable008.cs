using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceSettingTable008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDefaultCompany",
                schema: "MasterSettings",
                table: "InvoiceSettings",
                newName: "IsDefaultInvoiceSetting");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDefaultInvoiceSetting",
                schema: "MasterSettings",
                table: "InvoiceSettings",
                newName: "IsDefaultCompany");
        }
    }
}
