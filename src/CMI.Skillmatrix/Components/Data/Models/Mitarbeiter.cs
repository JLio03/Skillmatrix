namespace CMI.Skillmatrix.Components.Data.Models
{
    public class Mitarbeiter
    {
        public int MitarbeiterId { get; set; }
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
