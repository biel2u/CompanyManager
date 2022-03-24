using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyManager.Server.Data.Migrations
{
    public partial class AppointmentsOffersmanyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Offers_OfferId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_OfferId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Appointments");

            migrationBuilder.CreateTable(
                name: "AppointmentOffer",
                columns: table => new
                {
                    AppointmentsId = table.Column<int>(type: "int", nullable: false),
                    OffersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentOffer", x => new { x.AppointmentsId, x.OffersId });
                    table.ForeignKey(
                        name: "FK_AppointmentOffer_Appointments_AppointmentsId",
                        column: x => x.AppointmentsId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentOffer_Offers_OffersId",
                        column: x => x.OffersId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentOffer_OffersId",
                table: "AppointmentOffer",
                column: "OffersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentOffer");

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OfferId",
                table: "Appointments",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Offers_OfferId",
                table: "Appointments",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }
    }
}
