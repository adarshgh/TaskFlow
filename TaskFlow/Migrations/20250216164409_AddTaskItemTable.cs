using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TaskItems",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "IsComplete",
                table: "TaskItems",
                newName: "IsCompleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TaskItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TaskItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TaskItems");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TaskItems",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "TaskItems",
                newName: "IsComplete");
        }
    }
}
