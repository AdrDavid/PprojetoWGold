using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWow.Migrations
{
    /// <inheritdoc />
    public partial class AlteradoNomenclaturaDeServerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Servers",
                table: "Server",
                newName: "ServerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServerName",
                table: "Server",
                newName: "Servers");
        }
    }
}
