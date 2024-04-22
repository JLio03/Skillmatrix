using CMI.Skillmatrix.Components.Data.Models;

namespace CMI.Skillmatrix.Components.DTOs
{
    public class SkillDTO
    {
        public int SkillId { get; set; }
        public int SkillkategorieId { get; set; }
        public SkillkategorieDTO Skillkategorie { get; set; }
        public string SkillName { get; set; }
    }
}
