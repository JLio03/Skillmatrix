namespace CMI.Skillmatrix.Components.DTOs
{
    public class SkillkategorieMitSelbseinschaetzungDTO : SkillkategorieDTO
    {
        public List<SkillMitSelbsteinschaetzungDTO> SkillsMitSelbsteinschaetzung { get; set; } = [];
    }
}
