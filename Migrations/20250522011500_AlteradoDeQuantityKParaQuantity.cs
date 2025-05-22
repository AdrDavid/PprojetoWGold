using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWow.Migrations
{
    /// <inheritdoc />
    public partial class AlteradoDeQuantityKParaQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityK",
                table: "Order",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Order",
                newName: "QuantityK");
        }
    }
}
