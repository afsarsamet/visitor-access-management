using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class CikisLoglari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cikisVerenAdSoyad",
                table: "visitorLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cikisVerenSicilNo",
                table: "visitorLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cikisVerenAdSoyad",
                table: "visitorLog");

            migrationBuilder.DropColumn(
                name: "cikisVerenSicilNo",
                table: "visitorLog");
        }
    }
}
