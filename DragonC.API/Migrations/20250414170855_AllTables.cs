using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DragonC.API.Migrations
{
    /// <inheritdoc />
    public partial class AllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormalRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsStart = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormalRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HighLevelCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HighLevelCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LowLevelCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsConditional = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LowLevelCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenSeparators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Separator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenSeparators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProcessorFileId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    FormalRuleId = table.Column<int>(type: "int", nullable: true),
                    HighLevelCommandId = table.Column<int>(type: "int", nullable: true),
                    LowLevelCommandId = table.Column<int>(type: "int", nullable: true),
                    TokenSeparatorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_FormalRules_FormalRuleId",
                        column: x => x.FormalRuleId,
                        principalTable: "FormalRules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_HighLevelCommands_HighLevelCommandId",
                        column: x => x.HighLevelCommandId,
                        principalTable: "HighLevelCommands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_LowLevelCommands_LowLevelCommandId",
                        column: x => x.LowLevelCommandId,
                        principalTable: "LowLevelCommands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_TokenSeparators_TokenSeparatorId",
                        column: x => x.TokenSeparatorId,
                        principalTable: "TokenSeparators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FileId",
                table: "Projects",
                column: "FileId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "FormalRules");

            migrationBuilder.DropTable(
                name: "HighLevelCommands");

            migrationBuilder.DropTable(
                name: "LowLevelCommands");

            migrationBuilder.DropTable(
                name: "TokenSeparators");
        }
    }
}
