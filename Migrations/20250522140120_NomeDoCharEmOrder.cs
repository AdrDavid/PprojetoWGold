using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWow.Migrations
{
    /// <inheritdoc />
    public partial class NomeDoCharEmOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharName",
                table: "Order",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharName",
                table: "Order");
        }
    }
}
