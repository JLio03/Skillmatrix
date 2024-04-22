using CMI.Skillmatrix.Components.Data;
using CMI.Skillmatrix.Components.Data.Models;
using CMI.Skillmatrix.Components.DTOs;
using CMI.Skillmatrix.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CMI.Skillmatrix.Services
{
    public class SkillService(IDbContextFactory<SkillmatrixDbContext> dbContextFactory, IHttpContextAccessor contextAccessor)
    {
        // Holt alle Skills
        public async Task<List<SkillDTO>> GetAllSkillsAsync()
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                var skills = await context.Skill.ToListAsync();

                foreach (var skill in skills)
                {
                    skill.Skillkategorie = await GetSkillkategorieWhereSkillkategorieId(skill.SkillkategorieId);
                }

                return skills.Select(SkillMapper.MapToSkillDTO).ToList();
            }
        }

        // Holt die Skillkategorie anhand der SkillkategorieId
        public async Task<Skillkategorie> GetSkillkategorieWhereSkillkategorieId(int skillkategorieId)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                var skillkategorie = await context.Skillkategorie
                    .FirstOrDefaultAsync(sk => sk.SkillkategorieId == skillkategorieId);
                return skillkategorie;
            }
        }

        // Fügt einen neuen Skill hinzu
        public async Task<int> AddSkillAsync(SkillDTO skillDto)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    // Add new Skill
                    var skill = SkillMapper.MapToSkill(skillDto);
                    await context.Skill.AddAsync(skill);
                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich einen Skill erstellt", username);

                    return skill.SkillId;
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim erstellen eines neuen Skills", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }

        // Bearbeitet den Skill
        public async Task UpdateSkillAsync(SkillDTO skillDto)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var skill = await context.Skill.FindAsync(skillDto.SkillId);
                    skill.SkillkategorieId = skillDto.SkillkategorieId;
                    skill.SkillName = skillDto.SkillName;

                    context.Skill.Update(skill);

                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich einen Skill bearbeitet", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim bearbeiten eines neuen Skills", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }

        // Löscht den Skill
        public async Task DeleteSkillByIdAsync(int skillId)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var skill = context.Skill.Single(s => s.SkillId == skillId);
                    context.Skill.Remove(skill);
                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich einen Skill gelöscht", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim löschen eines neuen Skills", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }

        // Erstellt für den neuen Skill alle MitarbeiterSkill Einträge
        public async Task AddSkillToAllMitarbeiterAsync(int skillId)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var allMitarbeiter = await context.Mitarbeiter.ToListAsync();

                    foreach (var mitarbeiter in allMitarbeiter)
                    {
                        var mitarbeiterSkill = new MitarbeiterSkill
                        {
                            MitarbeiterId = mitarbeiter.MitarbeiterId,
                            SkillId = skillId,
                            Selbsteinschaetzung = 0
                        };

                        await context.AddAsync(mitarbeiterSkill);
                    }

                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich alle neuen MitarbeiterSkill Einträge aufgrund eines neuen Skills automatisch erfasst", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim erfassen neuer MitarbeiterSkill Einträge aufgrund eines neuen Skills", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }
    }
}
