using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class TekrarOlusuturma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SicilNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TCNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    sifre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "visitorLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ziyaretciFirma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ziyaretNedeni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kacKisi = table.Column<int>(type: "int", nullable: false),
                    plaka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    girisZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cikisZamani = table.Column<DateTime>(type: "datetime2", nullable: true),
                    asimVarMi = table.Column<bool>(type: "bit", nullable: false),
                    telefonNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ciktiMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visitorLog", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personel");

            migrationBuilder.DropTable(
                name: "visitorLog");
        }
    }
}
