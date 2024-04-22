using CMI.Skillmatrix.Components.Data;
using CMI.Skillmatrix.Components.Data.Models;
using CMI.Skillmatrix.Components.DTOs;
using CMI.Skillmatrix.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CMI.Skillmatrix.Services
{
    public class SkillkategorieService(IDbContextFactory<SkillmatrixDbContext> dbContextFactory, IHttpContextAccessor contextAccessor)
    {
        // Holt alle Skillkategorien
        public async Task<List<SkillkategorieDTO>> GetAllSkillkategorienAsync()
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                var skillkategories = await context.Skillkategorie.ToListAsync();
                return skillkategories.Select(SkillkategorieMapper.MapToSkillkategorieDto).ToList();
            }
        }

        // Fügt eine neue Skillkategorie hinzu
        public async Task AddSkillkategorieAsync(SkillkategorieDTO skillkategorieDto)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var skillkategorie = SkillkategorieMapper.MapToSkillkategorie(skillkategorieDto);
                    context.Skillkategorie.Add(skillkategorie);
                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich eine Skillkategorie erstellt", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim erstellen einer neuen Skillkategorie", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }

        // Bearbeitet die Skillkategorie
        public async Task UpdateSkillkategorieAsync(SkillkategorieDTO skillkategorieDto)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var skillkategorie = await context.Skillkategorie.FindAsync(skillkategorieDto.SkillkategorieId);
                    skillkategorie.SkillkategorieName = skillkategorieDto.SkillkategorieName;

                    context.Skillkategorie.Update(skillkategorie);

                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich eine Skillkategorie bearbeitet", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim bearbeiten einer Skillkategorie", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }

        // Löscht die Skillkategorie
        public async Task DeleteSkillkategorieByIdAsync(int skillkategorieId)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var skillkategorie = context.Skillkategorie.Single(s => s.SkillkategorieId == skillkategorieId);
                    context.Skillkategorie.Remove(skillkategorie);
                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich eine Skillkategorie gelöscht", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim löschen einer Skillkategorie", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }

        // Gibt die Anzahl Skills mit der Skillkategorie basierend auf der SkillkategorieId zurück
        public async Task<int> GetAmountOfSkillsWithSkillkategorieIdAsync(int skillkategorieId)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var countSkills = await context.Skill.CountAsync(skill => skill.SkillkategorieId == skillkategorieId);
                    return countSkills;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e}: An error occured returning the amount of skills with SkillkategorieID");
                    throw;
                }
            }
        }
    }
}
