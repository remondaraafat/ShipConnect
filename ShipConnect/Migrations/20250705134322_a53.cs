using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipConnect.Migrations
{
    /// <inheritdoc />
    public partial class a53 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "AspNetUsers",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "CompanyName");
        }
    }
}
