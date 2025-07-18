using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForgeHubApi.Migrations
{
    /// <inheritdoc />
    public partial class modelsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RFQs",
                columns: table => new
                {
                    RFQId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFQNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndentNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RFQLineNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReqQty = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UOM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReqDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FactoryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDateofBid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFQs", x => x.RFQId);
                    table.ForeignKey(
                        name: "FK_RFQs_Users_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RFQQuotations",
                columns: table => new
                {
                    QuotationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFQId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    BidNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentTerms = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFQQuotations", x => x.QuotationId);
                    table.ForeignKey(
                        name: "FK_RFQQuotations_RFQs_RFQId",
                        column: x => x.RFQId,
                        principalTable: "RFQs",
                        principalColumn: "RFQId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RFQQuotations_Users_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinalizedQuotations",
                columns: table => new
                {
                    FinalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFQId = table.Column<int>(type: "int", nullable: false),
                    QuotationId = table.Column<int>(type: "int", nullable: false),
                    FinalizedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalizedQuotations", x => x.FinalId);
                    table.ForeignKey(
                        name: "FK_FinalizedQuotations_RFQQuotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "RFQQuotations",
                        principalColumn: "QuotationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalizedQuotations_RFQs_RFQId",
                        column: x => x.RFQId,
                        principalTable: "RFQs",
                        principalColumn: "RFQId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalizedQuotations_QuotationId",
                table: "FinalizedQuotations",
                column: "QuotationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinalizedQuotations_RFQId",
                table: "FinalizedQuotations",
                column: "RFQId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RFQQuotations_RFQId",
                table: "RFQQuotations",
                column: "RFQId");

            migrationBuilder.CreateIndex(
                name: "IX_RFQQuotations_VendorId",
                table: "RFQQuotations",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_RFQs_BuyerId",
                table: "RFQs",
                column: "BuyerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalizedQuotations");

            migrationBuilder.DropTable(
                name: "RFQQuotations");

            migrationBuilder.DropTable(
                name: "RFQs");
        }
    }
}
