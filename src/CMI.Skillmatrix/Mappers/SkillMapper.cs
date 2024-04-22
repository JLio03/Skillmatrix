using CMI.Skillmatrix.Components.Data.Models;
using CMI.Skillmatrix.Components.DTOs;

namespace CMI.Skillmatrix.Mappers
{
    public class SkillMapper
    {
        public static Skill MapToSkill(SkillDTO skillDto)
        {
            var skill = new Skill
            {
                SkillName = skillDto.SkillName,
                SkillkategorieId = skillDto.Skillkategorie.SkillkategorieId
            };

            if (skillDto.SkillId > 0)
            {
                skill.SkillId = skillDto.SkillId;
            }

            if (skillDto.Skillkategorie != null && skillDto.SkillkategorieId > 0)
            {
                skill.Skillkategorie = SkillkategorieMapper.MapToSkillkategorie(skillDto.Skillkategorie);
            }

            return skill;
        }

        public static SkillDTO MapToSkillDTO(Skill skill)
        {
            if (skill == null)
            {
                return null;
            }

            return new SkillDTO
            {
                SkillId = skill.SkillId,
                SkillName = skill.SkillName,
                SkillkategorieId = skill.SkillkategorieId,
                Skillkategorie = SkillkategorieMapper.MapToSkillkategorieDto(skill.Skillkategorie)
            };
        }
    }
}
