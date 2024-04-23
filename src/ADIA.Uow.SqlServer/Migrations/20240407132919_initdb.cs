using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADIA.Uow.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analysis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    AnalysisDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analysis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisResponse",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    StartAnalysis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAnalysis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ResponseTime = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    Ia = table.Column<int>(type: "int", nullable: false),
                    IdAnalysis = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisResponse_Analysis_IdAnalysis",
                        column: x => x.IdAnalysis,
                        principalTable: "Analysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralText",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sentiment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdAnalysisResponse = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralText", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralText_AnalysisResponse_IdAnalysisResponse",
                        column: x => x.IdAnalysisResponse,
                        principalTable: "AnalysisResponse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SupplierAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TotalAmmount = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    IdAnalysisResponse = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_AnalysisResponse_IdAnalysisResponse",
                        column: x => x.IdAnalysisResponse,
                        principalTable: "AnalysisResponse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    IdInvoice = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemInvoice_Invoice_IdInvoice",
                        column: x => x.IdInvoice,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResponse_IdAnalysis",
                table: "AnalysisResponse",
                column: "IdAnalysis",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralText_IdAnalysisResponse",
                table: "GeneralText",
                column: "IdAnalysisResponse",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IdAnalysisResponse",
                table: "Invoice",
                column: "IdAnalysisResponse",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemInvoice_IdInvoice",
                table: "ItemInvoice",
                column: "IdInvoice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralText");

            migrationBuilder.DropTable(
                name: "ItemInvoice");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "AnalysisResponse");

            migrationBuilder.DropTable(
                name: "Analysis");
        }
    }
}
