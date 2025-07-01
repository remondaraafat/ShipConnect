using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PasswordResetCodes",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "PasswordResetCodes",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PasswordResetCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "PasswordResetCodes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PasswordResetCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PasswordResetCodes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PasswordResetCodes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PasswordResetCodes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PasswordResetCodes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PasswordResetCodes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PasswordResetCodes",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "PasswordResetCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);
        }
    }
}
