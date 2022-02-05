using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyManager.Server.Data.Migrations
{
    public partial class AddCoreTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(11)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contact = table.Column<bool>(type: "bit", nullable: false),
                    PublicImage = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consents_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TimeInMinutes = table.Column<int>(type: "int", nullable: false),
                    OfferCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_OfferCategories_OfferCategoryId",
                        column: x => x.OfferCategoryId,
                        principalTable: "OfferCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    FileName = table.Column<string>(type: "varchar(100)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.FileName);
                    table.ForeignKey(
                        name: "FK_Photos_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Photos_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OfferCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Konsultacje" },
                    { 2, "Zabiegi terapeutyczne" },
                    { 3, "Bioinżynieria tkankowa" },
                    { 4, "Mezoterapia mikroigłowa" },
                    { 5, "Mezoterapia igłowa" },
                    { 6, "Redermalizacja" },
                    { 7, "Stymulatory tkankowe" },
                    { 8, "Zabiegi na okolice oczu" },
                    { 9, "Zabiegi na skórę głowy" }
                });

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "Id", "Name", "OfferCategoryId", "Price", "TimeInMinutes" },
                values: new object[,]
                {
                    { 1, "Konsultacje 15-minutowa", 1, 0m, 15 },
                    { 2, "Konsultacja Beauty dla skór problematycznych", 1, 250m, 120 },
                    { 3, "Konsultacja Beauty dla skór starzejących się", 1, 200m, 120 },
                    { 4, "Konsultacja kontrolna", 1, 100m, 120 },
                    { 5, "Zabieg oczyszczający", 2, 250m, 90 },
                    { 6, "Zabieg z enzymami", 2, 250m, 90 },
                    { 7, "Zabieg retinolowy", 2, 400m, 90 },
                    { 8, "Zabieg z maską terapeutyczną", 2, 150m, 90 },
                    { 9, "Zabieg detoksykujący", 2, 250m, 90 },
                    { 10, "Peelingi chemiczne", 2, 300m, 90 },
                    { 11, "Sonoforeza", 3, 300m, 90 },
                    { 12, "Mezoterapia mikroigłowa", 3, 400m, 90 },
                    { 13, "Mezoterapia mikroigłowa", 4, 300m, 90 },
                    { 14, "Mezoterapia igłowa", 5, 400m, 90 },
                    { 15, "Hyalual Xela Rederm 1,1% (2ml) / Electri (1,5ml)", 6, 600m, 90 },
                    { 16, "Hyalual Xela Rederm 1,8 % (2ml)", 6, 700m, 90 },
                    { 17, "Hyalual Xela Rederm 2,2% (2ml)", 6, 800m, 90 },
                    { 18, "Nucleofill Medium / Strong", 7, 750m, 90 },
                    { 19, "Sunekos 200", 7, 600m, 90 },
                    { 20, "Mezoterapia mikroigłowa + ampułka", 8, 200m, 90 },
                    { 21, "Mezoterapia mikroigłowa + bioinżynieria tkankowa", 8, 250m, 90 },
                    { 22, "Mezoterapia igłowa Dermaheal / RRS HA", 8, 350m, 90 },
                    { 23, "Stymulator tkankowy Nucleofil Soft Eyes", 8, 750m, 90 },
                    { 24, "Stymulator tkankowy Sunekos 200", 8, 600m, 90 },
                    { 25, "Redermalizacja Electri", 8, 600m, 90 },
                    { 26, "Mezoterapia mikroigłowa + ampułka", 9, 250m, 90 },
                    { 27, "Mezoterapia igłowa RRS XL Hair", 9, 400m, 90 },
                    { 28, "Mezoterapia igłowa Dr Cyj Hair Filler", 9, 650m, 90 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OfferId",
                table: "Appointments",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Consents_CustomerId",
                table: "Consents",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_OfferCategoryId",
                table: "Offers",
                column: "OfferCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AppointmentId",
                table: "Photos",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CustomerId",
                table: "Photos",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consents");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "OfferCategories");
        }
    }
}
