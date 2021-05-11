using Microsoft.EntityFrameworkCore.Migrations;
using ScootAPI.Models;

namespace ScootAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scooter",
                columns: table => new
                {
                    idScooter = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    location = table.Column<Customer>(type: "jsonb", nullable: false),
                    isDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    idZone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("scooter_pkey", x => x.idScooter);
                });

            migrationBuilder.CreateTable(
                name: "zone",
                columns: table => new
                {
                    idZone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    isDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("zone_pkey", x => x.idZone);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scooter");

            migrationBuilder.DropTable(
                name: "zone");
        }
    }
}
