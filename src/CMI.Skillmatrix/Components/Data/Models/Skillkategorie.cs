namespace CMI.Skillmatrix.Components.Data.Models
{
    public class Skillkategorie
    {
        public int SkillkategorieId { get; set; }
        public string SkillkategorieName { get; set; }
        public virtual ICollection<Skill>? Skill { get; set; } = null;
    }
}
