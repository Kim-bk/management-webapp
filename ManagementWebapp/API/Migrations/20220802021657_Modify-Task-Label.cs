using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ModifyTaskLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelTask");

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Label",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Label_TaskId",
                table: "Label",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Label_Task_TaskId",
                table: "Label",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_Task_TaskId",
                table: "Label");

            migrationBuilder.DropIndex(
                name: "IX_Label_TaskId",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Label");

            migrationBuilder.CreateTable(
                name: "LabelTask",
                columns: table => new
                {
                    LabelId = table.Column<int>(type: "int", nullable: true),
                    TaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__LabelTask__Label__1DB06A4F",
                        column: x => x.LabelId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__LabelTask__TaskI__32E0915F",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelTask_LabelId",
                table: "LabelTask",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelTask_TaskId",
                table: "LabelTask",
                column: "TaskId");
        }
    }
}
