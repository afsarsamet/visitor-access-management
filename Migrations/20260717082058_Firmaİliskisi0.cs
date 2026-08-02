using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class Firmaİliskisi0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ziyaretciFirma",
                table: "visitorLog");

            migrationBuilder.AddColumn<int>(
                name: "FirmaId",
                table: "visitorLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_visitorLog_FirmaId",
                table: "visitorLog",
                column: "FirmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_visitorLog_Firmalar_FirmaId",
                table: "visitorLog",
                column: "FirmaId",
                principalTable: "Firmalar",
                principalColumn: "FirmaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visitorLog_Firmalar_FirmaId",
                table: "visitorLog");

            migrationBuilder.DropIndex(
                name: "IX_visitorLog_FirmaId",
                table: "visitorLog");

            migrationBuilder.DropColumn(
                name: "FirmaId",
                table: "visitorLog");

            migrationBuilder.AddColumn<string>(
                name: "ziyaretciFirma",
                table: "visitorLog",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
