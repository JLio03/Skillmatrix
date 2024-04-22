using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMI.Skillmatrix.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSkillkategorieIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Skillkategorie_KategorieId",
                table: "Skill");

            migrationBuilder.RenameColumn(
                name: "KategorieId",
                table: "Skill",
                newName: "SkillkategorieId");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_KategorieId",
                table: "Skill",
                newName: "IX_Skill_SkillkategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Skillkategorie_SkillkategorieId",
                table: "Skill",
                column: "SkillkategorieId",
                principalTable: "Skillkategorie",
                principalColumn: "SkillkategorieId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Skillkategorie_SkillkategorieId",
                table: "Skill");

            migrationBuilder.RenameColumn(
                name: "SkillkategorieId",
                table: "Skill",
                newName: "KategorieId");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_SkillkategorieId",
                table: "Skill",
                newName: "IX_Skill_KategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Skillkategorie_KategorieId",
                table: "Skill",
                column: "KategorieId",
                principalTable: "Skillkategorie",
                principalColumn: "SkillkategorieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
