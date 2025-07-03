using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipConnect.Migrations
{
    /// <inheritdoc />
    public partial class someEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "StartUps");

            migrationBuilder.DropColumn(
                name: "City",
                table: "ShippingCompanies");

            migrationBuilder.DropColumn(
                name: "DestinationCity",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ReceiverNotes",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "SenderCity",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Shipments");

            migrationBuilder.AddColumn<int>(
                name: "PackagingOptions",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackagingOptions",
                table: "Shipments");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "StartUps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ShippingCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationCity",
                table: "Shipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverNotes",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderCity",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Shipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
