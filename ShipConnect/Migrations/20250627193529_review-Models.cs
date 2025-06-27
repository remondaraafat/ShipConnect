using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipConnect.Migrations
{
    /// <inheritdoc />
    public partial class reviewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_ShippingCompanies_ShippingCompanyId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_StartUps_StartUpId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ShippingCompanies_ShippingCompanyId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_StartUps_StartUpId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_ApplicationUserId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_PayeeId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_PayerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Offers_ShippingOfferId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Shipments_ShipmentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_ShippingCompanies_ShippingCompanyId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_StartUps_StartUpId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedUserId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Shipments_ShipmentId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_AspNetUsers_ApplicationUserId",
                table: "Shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_ShippingCompanies_ShippingCompanyId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ApplicationUserId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_ShippingCompanyId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RatedUserId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId_RatedUserId_ShipmentId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PayeeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PayerId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ShippingCompanyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_StartUpId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ApplicationUserId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ShippingCompanyId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_StartUpId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_ShippingCompanyId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_StartUpId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingCompanyId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "RatedUserId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "PayeeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PayerId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ShippingCompanyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StartUpId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "ShippingCompanyId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "StartUpId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ShippingCompanyId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "StartUpId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ShipmentId",
                table: "Ratings",
                newName: "StartUpId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_ShipmentId",
                table: "Ratings",
                newName: "IX_Ratings_StartUpId");

            migrationBuilder.RenameColumn(
                name: "ShippingOfferId",
                table: "Payments",
                newName: "SenderBankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ShippingOfferId",
                table: "Payments",
                newName: "IX_Payments_SenderBankAccountId");

            migrationBuilder.AddColumn<int>(
                name: "ShipingCompanyId",
                table: "Trackings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReceiverBankAccountId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BankAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Trackings_ShipingCompanyId",
                table: "Trackings",
                column: "ShipingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CompanyId",
                table: "Ratings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_OfferId",
                table: "Ratings",
                column: "OfferId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReceiverBankAccountId",
                table: "Payments",
                column: "ReceiverBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId",
                table: "BankAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_BankAccounts_ReceiverBankAccountId",
                table: "Payments",
                column: "ReceiverBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_BankAccounts_SenderBankAccountId",
                table: "Payments",
                column: "SenderBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Shipments_ShipmentId",
                table: "Payments",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Offers_OfferId",
                table: "Ratings",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_ShippingCompanies_CompanyId",
                table: "Ratings",
                column: "CompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_StartUps_StartUpId",
                table: "Ratings",
                column: "StartUpId",
                principalTable: "StartUps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trackings_ShippingCompanies_ShipingCompanyId",
                table: "Trackings",
                column: "ShipingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_BankAccounts_ReceiverBankAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_BankAccounts_SenderBankAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Shipments_ShipmentId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Offers_OfferId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_ShippingCompanies_CompanyId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_StartUps_StartUpId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trackings_ShippingCompanies_ShipingCompanyId",
                table: "Trackings");

            migrationBuilder.DropIndex(
                name: "IX_Trackings_ShipingCompanyId",
                table: "Trackings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_CompanyId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_OfferId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ReceiverBankAccountId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "ShipingCompanyId",
                table: "Trackings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReceiverBankAccountId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "StartUpId",
                table: "Ratings",
                newName: "ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_StartUpId",
                table: "Ratings",
                newName: "IX_Ratings_ShipmentId");

            migrationBuilder.RenameColumn(
                name: "SenderBankAccountId",
                table: "Payments",
                newName: "ShippingOfferId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_SenderBankAccountId",
                table: "Payments",
                newName: "IX_Payments_ShippingOfferId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Shipments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShippingCompanyId",
                table: "Shipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RatedUserId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayeeId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayerId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ShippingCompanyId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartUpId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Offers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShippingCompanyId",
                table: "ChatMessages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartUpId",
                table: "ChatMessages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShippingCompanyId",
                table: "BankAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartUpId",
                table: "BankAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ApplicationUserId",
                table: "Shipments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShippingCompanyId",
                table: "Shipments",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RatedUserId",
                table: "Ratings",
                column: "RatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_RatedUserId_ShipmentId",
                table: "Ratings",
                columns: new[] { "UserId", "RatedUserId", "ShipmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PayeeId",
                table: "Payments",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PayerId",
                table: "Payments",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ShippingCompanyId",
                table: "Payments",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StartUpId",
                table: "Payments",
                column: "StartUpId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ApplicationUserId",
                table: "Offers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ShippingCompanyId",
                table: "ChatMessages",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_StartUpId",
                table: "ChatMessages",
                column: "StartUpId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ShippingCompanyId",
                table: "BankAccounts",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_StartUpId",
                table: "BankAccounts",
                column: "StartUpId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_ShippingCompanies_ShippingCompanyId",
                table: "BankAccounts",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_StartUps_StartUpId",
                table: "BankAccounts",
                column: "StartUpId",
                principalTable: "StartUps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ShippingCompanies_ShippingCompanyId",
                table: "ChatMessages",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_StartUps_StartUpId",
                table: "ChatMessages",
                column: "StartUpId",
                principalTable: "StartUps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_ApplicationUserId",
                table: "Offers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_PayeeId",
                table: "Payments",
                column: "PayeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_PayerId",
                table: "Payments",
                column: "PayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Offers_ShippingOfferId",
                table: "Payments",
                column: "ShippingOfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Shipments_ShipmentId",
                table: "Payments",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_ShippingCompanies_ShippingCompanyId",
                table: "Payments",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_StartUps_StartUpId",
                table: "Payments",
                column: "StartUpId",
                principalTable: "StartUps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedUserId",
                table: "Ratings",
                column: "RatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId",
                table: "Ratings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Shipments_ShipmentId",
                table: "Ratings",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_AspNetUsers_ApplicationUserId",
                table: "Shipments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_ShippingCompanies_ShippingCompanyId",
                table: "Shipments",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id");
        }
    }
}
