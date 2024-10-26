using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoolLicensing.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class IndexProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductCode",
                table: "Products");
        }
    }
}
