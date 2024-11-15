using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Migrations
{
    /// <inheritdoc />
    public partial class Init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Task_ProjectTaskId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_ExecutorId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Category_CategoryId",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "ProjectTask");

            migrationBuilder.RenameIndex(
                name: "IX_Task_ExecutorId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_ExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_CategoryId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTask",
                table: "ProjectTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_ProjectTask_ProjectTaskId",
                table: "Comment",
                column: "ProjectTaskId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_AspNetUsers_ExecutorId",
                table: "ProjectTask",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Category_CategoryId",
                table: "ProjectTask",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_ProjectTask_ProjectTaskId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_AspNetUsers_ExecutorId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Category_CategoryId",
                table: "ProjectTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTask",
                table: "ProjectTask");

            migrationBuilder.RenameTable(
                name: "ProjectTask",
                newName: "Task");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_ExecutorId",
                table: "Task",
                newName: "IX_Task_ExecutorId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_CategoryId",
                table: "Task",
                newName: "IX_Task_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Task_ProjectTaskId",
                table: "Comment",
                column: "ProjectTaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_ExecutorId",
                table: "Task",
                column: "ExecutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Category_CategoryId",
                table: "Task",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
