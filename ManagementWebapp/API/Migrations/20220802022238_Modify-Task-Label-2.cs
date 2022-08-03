using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ModifyTaskLabel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    LabelsId = table.Column<int>(type: "int", nullable: false),
                    TasksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelTask", x => new { x.LabelsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_LabelTask_Label_LabelsId",
                        column: x => x.LabelsId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelTask_Task_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelTask_TasksId",
                table: "LabelTask",
                column: "TasksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
