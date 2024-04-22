namespace CMI.Skillmatrix.Components.Data.Models
{
    public class MitarbeiterSkill
    {
        public int MitarbeiterId { get; set; }
        public int SkillId { get; set; }
        public int Selbsteinschaetzung { get; set; }
        public virtual Mitarbeiter Mitarbeiter { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
