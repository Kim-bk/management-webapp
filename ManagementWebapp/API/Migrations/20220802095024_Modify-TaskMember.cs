using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ModifyTaskMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Task__UserId__160F4887",
                table: "Task");

            migrationBuilder.DropTable(
                name: "TaskMember");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Task",
                newName: "DoingId");

            migrationBuilder.CreateTable(
                name: "ApplicationUserTask",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTask", x => new { x.TasksId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserTask_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTask_Task_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTask_UsersId",
                table: "ApplicationUserTask",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserTask");

            migrationBuilder.RenameColumn(
                name: "DoingId",
                table: "Task",
                newName: "UserId");

            migrationBuilder.CreateTable(
                name: "TaskMember",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId1 = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskMember", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_TaskMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskMember_Task_TaskId1",
                        column: x => x.TaskId1,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskMember_TaskId1",
                table: "TaskMember",
                column: "TaskId1");

            migrationBuilder.CreateIndex(
                name: "IX_TaskMember_UserId",
                table: "TaskMember",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__Task__UserId__160F4887",
                table: "Task",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
