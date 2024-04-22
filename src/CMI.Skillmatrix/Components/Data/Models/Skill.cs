namespace CMI.Skillmatrix.Components.Data.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        public virtual ICollection<Mitarbeiter> Mitarbeiter { get; set; } = new List<Mitarbeiter>();
        public int SkillkategorieId { get; set; }
        public virtual Skillkategorie Skillkategorie { get; set; }
        public string SkillName { get; set; }
    }
}
