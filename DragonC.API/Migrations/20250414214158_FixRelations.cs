using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragonC.API.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_FormalRules_FormalRuleId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_HighLevelCommands_HighLevelCommandId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_LowLevelCommands_LowLevelCommandId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_TokenSeparators_TokenSeparatorId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FormalRuleId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_HighLevelCommandId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_LowLevelCommandId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_TokenSeparatorId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FormalRuleId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "HighLevelCommandId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LowLevelCommandId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TokenSeparatorId",
                table: "Projects");

            migrationBuilder.CreateIndex(
                name: "IX_TokenSeparators_ProjectId",
                table: "TokenSeparators",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LowLevelCommands_ProjectId",
                table: "LowLevelCommands",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_HighLevelCommands_ProjectId",
                table: "HighLevelCommands",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FormalRules_ProjectId",
                table: "FormalRules",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormalRules_Projects_ProjectId",
                table: "FormalRules",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HighLevelCommands_Projects_ProjectId",
                table: "HighLevelCommands",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LowLevelCommands_Projects_ProjectId",
                table: "LowLevelCommands",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TokenSeparators_Projects_ProjectId",
                table: "TokenSeparators",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormalRules_Projects_ProjectId",
                table: "FormalRules");

            migrationBuilder.DropForeignKey(
                name: "FK_HighLevelCommands_Projects_ProjectId",
                table: "HighLevelCommands");

            migrationBuilder.DropForeignKey(
                name: "FK_LowLevelCommands_Projects_ProjectId",
                table: "LowLevelCommands");

            migrationBuilder.DropForeignKey(
                name: "FK_TokenSeparators_Projects_ProjectId",
                table: "TokenSeparators");

            migrationBuilder.DropIndex(
                name: "IX_TokenSeparators_ProjectId",
                table: "TokenSeparators");

            migrationBuilder.DropIndex(
                name: "IX_LowLevelCommands_ProjectId",
                table: "LowLevelCommands");

            migrationBuilder.DropIndex(
                name: "IX_HighLevelCommands_ProjectId",
                table: "HighLevelCommands");

            migrationBuilder.DropIndex(
                name: "IX_FormalRules_ProjectId",
                table: "FormalRules");

            migrationBuilder.AddColumn<int>(
                name: "FormalRuleId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HighLevelCommandId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LowLevelCommandId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TokenSeparatorId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FormalRuleId",
                table: "Projects",
                column: "FormalRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_HighLevelCommandId",
                table: "Projects",
                column: "HighLevelCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LowLevelCommandId",
                table: "Projects",
                column: "LowLevelCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TokenSeparatorId",
                table: "Projects",
                column: "TokenSeparatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_FormalRules_FormalRuleId",
                table: "Projects",
                column: "FormalRuleId",
                principalTable: "FormalRules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_HighLevelCommands_HighLevelCommandId",
                table: "Projects",
                column: "HighLevelCommandId",
                principalTable: "HighLevelCommands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_LowLevelCommands_LowLevelCommandId",
                table: "Projects",
                column: "LowLevelCommandId",
                principalTable: "LowLevelCommands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_TokenSeparators_TokenSeparatorId",
                table: "Projects",
                column: "TokenSeparatorId",
                principalTable: "TokenSeparators",
                principalColumn: "Id");
        }
    }
}
