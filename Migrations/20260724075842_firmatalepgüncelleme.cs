using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class firmatalepgüncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReddedildiMi",
                table: "FirmaTalepleri");

            migrationBuilder.AlterColumn<bool>(
                name: "OnaylandiMi",
                table: "FirmaTalepleri",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "OnaylandiMi",
                table: "FirmaTalepleri",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReddedildiMi",
                table: "FirmaTalepleri",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
