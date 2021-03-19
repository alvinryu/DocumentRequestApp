using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_T_Request");

            migrationBuilder.RenameColumn(
                name: "RequestID",
                table: "Tb_T_DetailRequest",
                newName: "DetailRequestID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DetailRequestID",
                table: "Tb_T_DetailRequest",
                newName: "RequestID");

            migrationBuilder.CreateTable(
                name: "Tb_T_Request",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeTypeID = table.Column<int>(type: "int", nullable: true),
                    PersonNIK = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tb_T_Request_Tb_M_Person_PersonNIK",
                        column: x => x.PersonNIK,
                        principalTable: "Tb_M_Person",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tb_T_Request_Tb_T_DetailRequest_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Tb_T_DetailRequest",
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
    }
}
