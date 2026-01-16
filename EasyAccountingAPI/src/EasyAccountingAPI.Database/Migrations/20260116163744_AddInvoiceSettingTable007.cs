using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceSettingTable007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceSettings",
                schema: "MasterSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceDueDateCount = table.Column<int>(type: "int", nullable: false),
                    InvoiceColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceFooter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHideInvoiceHeader = table.Column<bool>(type: "bit", nullable: false),
                    IsShowPreviousDue = table.Column<bool>(type: "bit", nullable: false),
                    IsShowInvoiceCreatorName = table.Column<bool>(type: "bit", nullable: false),
                    IsShowCustomerSignature = table.Column<bool>(type: "bit", nullable: false),
                    IsCreateInvoiceWithoutPurchase = table.Column<bool>(type: "bit", nullable: false),
                    IsServiceProviderAttributionUnderInvoice = table.Column<bool>(type: "bit", nullable: false),
                    IsDefaultCompany = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceSettings",
                schema: "MasterSettings");
        }
    }
}
