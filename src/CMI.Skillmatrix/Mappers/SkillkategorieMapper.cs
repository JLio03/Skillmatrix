using CMI.Skillmatrix.Components.Data.Models;
using CMI.Skillmatrix.Components.DTOs;

namespace CMI.Skillmatrix.Mappers
{
    public class SkillkategorieMapper
    {
        public static Skillkategorie MapToSkillkategorie(SkillkategorieDTO skillkategorieDto)
        {
            if (skillkategorieDto == null)
            {
                return null;
            }

            return new Skillkategorie
            {
                SkillkategorieId = skillkategorieDto.SkillkategorieId,
                SkillkategorieName = skillkategorieDto.SkillkategorieName
            };
        }

        public static SkillkategorieDTO MapToSkillkategorieDto(Skillkategorie skillkategorie)
        {
            if (skillkategorie == null)
            {
                return null;
            }

            return new SkillkategorieDTO()
            {
                SkillkategorieId = skillkategorie.SkillkategorieId,
                SkillkategorieName = skillkategorie.SkillkategorieName
            };
        }
    }
}
