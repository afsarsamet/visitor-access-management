using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class FirmaTalepGuncellemeleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReddedildiMi",
                table: "FirmaTalepleri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TalepEdenAdSoyad",
                table: "FirmaTalepleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TalepEdenSicilNo",
                table: "FirmaTalepleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReddedildiMi",
                table: "FirmaTalepleri");

            migrationBuilder.DropColumn(
                name: "TalepEdenAdSoyad",
                table: "FirmaTalepleri");

            migrationBuilder.DropColumn(
                name: "TalepEdenSicilNo",
                table: "FirmaTalepleri");
        }
    }
}
