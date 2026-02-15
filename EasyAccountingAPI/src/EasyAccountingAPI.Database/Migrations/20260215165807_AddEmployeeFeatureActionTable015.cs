using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeFeatureActionTable015 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Authentication",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeFeatureActions",
                schema: "MasterSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFeatureActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeFeatureActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "MasterSettings",
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeFeatureActions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Authentication",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeFeatureActions_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "MasterSettings",
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                schema: "Authentication",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFeatureActions_ActionId",
                schema: "MasterSettings",
                table: "EmployeeFeatureActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFeatureActions_EmployeeId",
                schema: "MasterSettings",
                table: "EmployeeFeatureActions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFeatureActions_FeatureId",
                schema: "MasterSettings",
                table: "EmployeeFeatureActions",
                column: "FeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                schema: "Authentication",
                table: "Employees",
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
                name: "FK_Employees_Companies_CompanyId",
                schema: "Authentication",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeFeatureActions",
                schema: "MasterSettings");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CompanyId",
                schema: "Authentication",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Authentication",
                table: "Employees");
        }
    }
}
