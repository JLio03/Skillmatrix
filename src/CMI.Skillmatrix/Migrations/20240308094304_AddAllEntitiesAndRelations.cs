using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMI.Skillmatrix.Migrations
{
    /// <inheritdoc />
    public partial class AddAllEntitiesAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mitarbeiter",
                columns: table => new
                {
                    MitarbeiterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vorname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mitarbeiter", x => x.MitarbeiterId);
                });

            migrationBuilder.CreateTable(
                name: "Skillkategorie",
                columns: table => new
                {
                    SkillkategorieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillkategorieName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skillkategorie", x => x.SkillkategorieId);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategorieId = table.Column<int>(type: "int", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_Skill_Skillkategorie_KategorieId",
                        column: x => x.KategorieId,
                        principalTable: "Skillkategorie",
                        principalColumn: "SkillkategorieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MitarbeiterSkill",
                columns: table => new
                {
                    MitarbeiterId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    Selbsteinschaetzung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MitarbeiterSkill", x => new { x.MitarbeiterId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_MitarbeiterSkill_Mitarbeiter_MitarbeiterId",
                        column: x => x.MitarbeiterId,
                        principalTable: "Mitarbeiter",
                        principalColumn: "MitarbeiterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MitarbeiterSkill_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MitarbeiterSkill_SkillId",
                table: "MitarbeiterSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_KategorieId",
                table: "Skill",
                column: "KategorieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MitarbeiterSkill");

            migrationBuilder.DropTable(
                name: "Mitarbeiter");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "Skillkategorie");
        }
    }
}
