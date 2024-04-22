using CMI.Skillmatrix.Components.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CMI.Skillmatrix.Components.Data
{
    public class SkillmatrixDbContext : DbContext
    {
        public DbSet<Mitarbeiter> Mitarbeiter { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<Skillkategorie> Skillkategorie { get; set; }
        public DbSet<MitarbeiterSkill> MitarbeiterSkill { get; set; }

        public SkillmatrixDbContext(DbContextOptions<SkillmatrixDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MitarbeiterSkill>().ToTable("MitarbeiterSkill");

            // Mitarbeiter and MitarbeiterSkill
            modelBuilder.Entity<Mitarbeiter>().HasKey(u => u.MitarbeiterId);
            modelBuilder.Entity<Mitarbeiter>()
                .HasMany(m => m.Skills)
                .WithMany(s => s.Mitarbeiter)
                .UsingEntity<MitarbeiterSkill>();
            modelBuilder.Entity<Mitarbeiter>().Property(u => u.Name).IsRequired();
            modelBuilder.Entity<Mitarbeiter>().Property(u => u.Vorname).IsRequired();
            modelBuilder.Entity<Mitarbeiter>().Property(u => u.Email).IsRequired();

            // Skill
            modelBuilder.Entity<Skill>().HasKey(s => s.SkillId);
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Skillkategorie)
                .WithMany(s => s.Skill)
                .HasForeignKey(sc => sc.SkillkategorieId);
            modelBuilder.Entity<Skill>().Property(u => u.SkillName).IsRequired();

            // Skillkategorie
            modelBuilder.Entity<Skillkategorie>().HasKey(sc => sc.SkillkategorieId);
            modelBuilder.Entity<Skillkategorie>().Property(sc => sc.SkillkategorieName).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
