using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_T_Request",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonNIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentTypeTypeID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_T_Request", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_Tb_T_Request_Tb_M_DocumentType_DocumentTypeTypeID",
                        column: x => x.DocumentTypeTypeID,
                        principalTable: "Tb_M_DocumentType",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_T_Request_Tb_M_Person_PersonNIK",
                        column: x => x.PersonNIK,
                        principalTable: "Tb_M_Person",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_T_DetailRequest",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    ApproveRM = table.Column<int>(type: "int", nullable: false),
                    ApproveHR = table.Column<int>(type: "int", nullable: false),
                    HR_NIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveRMDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveHRDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_T_DetailRequest", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_Tb_T_DetailRequest_Tb_T_Request_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Tb_T_Request",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_T_Request_DocumentTypeTypeID",
                table: "Tb_T_Request",
                column: "DocumentTypeTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_T_Request_PersonNIK",
                table: "Tb_T_Request",
                column: "PersonNIK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_T_DetailRequest");

            migrationBuilder.DropTable(
                name: "Tb_T_Request");
        }
    }
}
