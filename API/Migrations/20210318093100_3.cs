using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NIK",
                table: "Tb_T_Request");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NIK",
                table: "Tb_T_Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
