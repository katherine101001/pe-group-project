using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixProjectTaskUserFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Users_AssignToUserId",
                table: "ProjectTask");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssignToUserId",
                table: "ProjectTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Users_AssignToUserId",
                table: "ProjectTask",
                column: "AssignToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Users_AssignToUserId",
                table: "ProjectTask");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssignToUserId",
                table: "ProjectTask",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Users_AssignToUserId",
                table: "ProjectTask",
                column: "AssignToUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
