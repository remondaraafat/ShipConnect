using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipConnect.Migrations
{
    /// <inheritdoc />
    public partial class a44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AspNetUsers",
                newName: "IsApproved");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "AspNetUsers",
                newName: "IsActive");
        }
    }
}
