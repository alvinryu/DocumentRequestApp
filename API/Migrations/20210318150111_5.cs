using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_T_DetailRequest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_T_DetailRequest",
                columns: table => new
                {
                    DetailRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApproveHR = table.Column<int>(type: "int", nullable: false),
                    ApproveHRDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveRM = table.Column<int>(type: "int", nullable: false),
                    ApproveRMDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HR_NIK = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_T_DetailRequest", x => x.DetailRequestID);
                });
        }
    }
}
