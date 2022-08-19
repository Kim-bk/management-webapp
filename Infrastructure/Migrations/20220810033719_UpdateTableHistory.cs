using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class UpdateTableHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "Histories");

            migrationBuilder.RenameColumn(
                name: "AtTable",
                table: "Histories",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "Histories",
                newName: "TableName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Histories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AffectedColumn",
                table: "Histories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewValues",
                table: "Histories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValues",
                table: "Histories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimaryKey",
                table: "Histories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Histories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AffectedColumn",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "NewValues",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "OldValues",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "PrimaryKey",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Histories");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Histories",
                newName: "AtTable");

            migrationBuilder.RenameColumn(
                name: "TableName",
                table: "Histories",
                newName: "Action");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Histories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Histories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "Histories",
                type: "int",
                nullable: true);
        }
    }
}
