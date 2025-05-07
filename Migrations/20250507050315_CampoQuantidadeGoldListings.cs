using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWow.Migrations
{
    /// <inheritdoc />
    public partial class CampoQuantidadeGoldListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Qtd",
                table: "GoldListing",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qtd",
                table: "GoldListing");
        }
    }
}
