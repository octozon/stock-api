using Microsoft.EntityFrameworkCore.Migrations;

namespace Stocks.Infrastructure.EntityFrameworkCore.Migrations
{
    public partial class FKStockItems2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockItems_Stocks_StockId",
                table: "StockItems");

            migrationBuilder.DropIndex(
                name: "IX_StockItems_StockId",
                table: "StockItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StockItems_StockId",
                table: "StockItems",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockItems_Stocks_StockId",
                table: "StockItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
