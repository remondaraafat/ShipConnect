using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipConnect.Migrations
{
    /// <inheritdoc />
    public partial class afterAddSomeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Website",
                table: "StartUps");

            migrationBuilder.RenameColumn(
                name: "Industry",
                table: "StartUps",
                newName: "City");

            migrationBuilder.AddColumn<string>(
                name: "BusinessCategory",
                table: "StartUps",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "ShippingCompanies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessCategory",
                table: "StartUps");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "StartUps",
                newName: "Industry");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "StartUps",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "ShippingCompanies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
