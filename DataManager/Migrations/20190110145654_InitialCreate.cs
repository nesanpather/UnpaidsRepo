using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataManager.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    Notification = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Notification", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "tb_Response",
                columns: table => new
                {
                    ResponseId = table.Column<int>(nullable: false),
                    Response = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Response", x => x.ResponseId);
                });

            migrationBuilder.CreateTable(
                name: "tb_Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "tb_Unpaid",
                columns: table => new
                {
                    UnpaidId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PolicyNumber = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(maxLength: 200, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    IdNumber = table.Column<string>(maxLength: 50, nullable: false),
                    IdempotencyKey = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Unpaid", x => x.UnpaidId);
                });

            migrationBuilder.CreateTable(
                name: "tb_UnpaidRequest",
                columns: table => new
                {
                    UnpaidRequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UnpaidId = table.Column<int>(nullable: false),
                    NotificationId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    StatusAdditionalInfo = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_UnpaidRequest", x => x.UnpaidRequestId);
                    table.ForeignKey(
                        name: "FK_tb_UnpaidRequest_tb_Notification",
                        column: x => x.NotificationId,
                        principalTable: "tb_Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_UnpaidRequest_tb_Status",
                        column: x => x.StatusId,
                        principalTable: "tb_Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_UnpaidRequest_tb_Unpaid",
                        column: x => x.UnpaidId,
                        principalTable: "tb_Unpaid",
                        principalColumn: "UnpaidId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_UnpaidResponse",
                columns: table => new
                {
                    UnpaidResponseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UnpaidRequestId = table.Column<int>(nullable: false),
                    ResponseId = table.Column<int>(nullable: false),
                    Accepted = table.Column<bool>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_UnpaidResponse", x => x.UnpaidResponseId);
                    table.ForeignKey(
                        name: "FK_tb_UnpaidResponse_tb_Response",
                        column: x => x.ResponseId,
                        principalTable: "tb_Response",
                        principalColumn: "ResponseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_UnpaidResponse_tb_Status",
                        column: x => x.StatusId,
                        principalTable: "tb_Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_UnpaidResponse_tb_UnpaidRequest",
                        column: x => x.UnpaidRequestId,
                        principalTable: "tb_UnpaidRequest",
                        principalColumn: "UnpaidRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_UnpaidRequest_NotificationId",
                table: "tb_UnpaidRequest",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_UnpaidRequest_StatusId",
                table: "tb_UnpaidRequest",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_UnpaidRequest_UnpaidId",
                table: "tb_UnpaidRequest",
                column: "UnpaidId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_UnpaidResponse_ResponseId",
                table: "tb_UnpaidResponse",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_UnpaidResponse_StatusId",
                table: "tb_UnpaidResponse",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_UnpaidResponse_UnpaidRequestId",
                table: "tb_UnpaidResponse",
                column: "UnpaidRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_UnpaidResponse");

            migrationBuilder.DropTable(
                name: "tb_Response");

            migrationBuilder.DropTable(
                name: "tb_UnpaidRequest");

            migrationBuilder.DropTable(
                name: "tb_Notification");

            migrationBuilder.DropTable(
                name: "tb_Status");

            migrationBuilder.DropTable(
                name: "tb_Unpaid");
        }
    }
}
