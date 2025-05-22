using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWow.Migrations
{
    /// <inheritdoc />
    public partial class AddUmCampoDeIdParaCodigoDoStripeNoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChaveVendedor",
                table: "User",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChaveVendedor",
                table: "User");
        }
    }
}
