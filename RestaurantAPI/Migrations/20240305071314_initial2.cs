using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PMethod",
                table: "OrderMasters",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "GTotal",
                table: "OrderMasters",
                newName: "GrandTotal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "OrderMasters",
                newName: "PMethod");

            migrationBuilder.RenameColumn(
                name: "GrandTotal",
                table: "OrderMasters",
                newName: "GTotal");
        }
    }
}
