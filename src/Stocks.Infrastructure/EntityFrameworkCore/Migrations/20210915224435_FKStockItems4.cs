using Microsoft.EntityFrameworkCore.Migrations;

namespace Stocks.Infrastructure.EntityFrameworkCore.Migrations
{
    public partial class FKStockItems4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockItems_Stocks_Id",
                table: "StockItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_StockItems_Stocks_Id",
                table: "StockItems",
                column: "Id",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
