using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_M_Person_Tb_M_Department_DepartmentID",
                table: "Tb_M_Person");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Tb_M_Person",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_M_Person_Tb_M_Department_DepartmentID",
                table: "Tb_M_Person",
                column: "DepartmentID",
                principalTable: "Tb_M_Department",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_M_Person_Tb_M_Department_DepartmentID",
                table: "Tb_M_Person");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Tb_M_Person",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_M_Person_Tb_M_Department_DepartmentID",
                table: "Tb_M_Person",
                column: "DepartmentID",
                principalTable: "Tb_M_Department",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
