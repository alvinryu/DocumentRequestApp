using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_M_Department",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manager_NIK = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_Department", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Tb_M_DocumentType",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_DocumentType", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "Tb_M_Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Tb_T_DetailRequest",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApproveRM = table.Column<int>(type: "int", nullable: false),
                    ApproveHR = table.Column<int>(type: "int", nullable: false),
                    HR_NIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveRMDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveHRDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_T_DetailRequest", x => x.RequestID);
                });

            migrationBuilder.CreateTable(
                name: "Tb_M_Person",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KTP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_Person", x => x.NIK);
                    table.ForeignKey(
                        name: "FK_Tb_M_Person_Tb_M_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Tb_M_Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tb_M_Account",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_Account", x => x.NIK);
                    table.ForeignKey(
                        name: "FK_Tb_M_Account_Tb_M_Person_NIK",
                        column: x => x.NIK,
                        principalTable: "Tb_M_Person",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_T_Request",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    NIK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonNIK = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TypeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_T_Request", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_Tb_T_Request_Tb_M_DocumentType_TypeID",
                        column: x => x.TypeID,
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

            migrationBuilder.CreateTable(
                name: "Tb_T_AccountRole",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_T_AccountRole", x => new { x.NIK, x.RoleID });
                    table.ForeignKey(
                        name: "FK_Tb_T_AccountRole_Tb_M_Account_NIK",
                        column: x => x.NIK,
                        principalTable: "Tb_M_Account",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_T_AccountRole_Tb_M_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Tb_M_Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_M_Person_DepartmentID",
                table: "Tb_M_Person",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_T_AccountRole_RoleID",
                table: "Tb_T_AccountRole",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_T_Request_TypeID",
                table: "Tb_T_Request",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_T_Request_PersonNIK",
                table: "Tb_T_Request",
                column: "PersonNIK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_T_AccountRole");

            migrationBuilder.DropTable(
                name: "Tb_T_Request");

            migrationBuilder.DropTable(
                name: "Tb_M_Account");

            migrationBuilder.DropTable(
                name: "Tb_M_Role");

            migrationBuilder.DropTable(
                name: "Tb_M_DocumentType");

            migrationBuilder.DropTable(
                name: "Tb_T_DetailRequest");

            migrationBuilder.DropTable(
                name: "Tb_M_Person");

            migrationBuilder.DropTable(
                name: "Tb_M_Department");
        }
    }
}
