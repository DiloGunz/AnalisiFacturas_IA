using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADIA.Uow.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Db2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ResponseTime",
                table: "AnalysisResponse",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ResponseTime",
                table: "AnalysisResponse",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
