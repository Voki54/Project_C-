using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Manager.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProjectId_to_ProjectTask1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Projects_ProjectId",
                table: "ProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_ProjectId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectTask");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_ProjectId",
                table: "Category",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Projects_ProjectId",
                table: "Category",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Projects_ProjectId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_ProjectId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_ProjectId",
                table: "ProjectTask",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Projects_ProjectId",
                table: "ProjectTask",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
