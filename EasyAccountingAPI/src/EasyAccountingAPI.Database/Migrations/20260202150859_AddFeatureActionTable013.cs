using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAccountingAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureActionTable013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeatureActions",
                schema: "MasterSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "MasterSettings",
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeatureActions_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "MasterSettings",
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureActions_ActionId",
                schema: "MasterSettings",
                table: "FeatureActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureActions_FeatureId",
                schema: "MasterSettings",
                table: "FeatureActions",
                column: "FeatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureActions",
                schema: "MasterSettings");
        }
    }
}
