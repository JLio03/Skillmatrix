using System.Diagnostics;
using CMI.Skillmatrix.Components.Data;
using CMI.Skillmatrix.Components.Data.Models;
using CMI.Skillmatrix.Components.DTOs;
using CMI.Skillmatrix.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CMI.Skillmatrix.Services
{
    public class MitarbeiterService(IDbContextFactory<SkillmatrixDbContext> dbContextFactory, IHttpContextAccessor contextAccessor)
    {
        // Holt alle Mitarbeiter
        public async Task<List<MitarbeiterDTO>> GetAllMitarbeiterAsync()
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                var mitarbeiter = await context.Mitarbeiter.ToListAsync();
                return mitarbeiter.Select(MitarbeiterMapper.MapToMitarbeiterDTO).ToList();
            }
        }

        // Fügt einen neuen Mitarbeiter hinzu
        public async Task<int> AddMitarbeiterAsync(MitarbeiterDTO mitarbeiterDto)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var mitarbeiter = MitarbeiterMapper.MapToMitarbeiter(mitarbeiterDto);
                    await context.Mitarbeiter.AddAsync(mitarbeiter);
                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich einen Mitarbeiter erstellt", username);

                    return mitarbeiter.MitarbeiterId;
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{Username}: Es gab einen Fehler beim Hinzufügen eines neuen Mitarbeiters", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }

        // Bearbeitet den Mitarbeiter
        public async Task UpdateMitarbeiterAsync(MitarbeiterDTO mitarbeiterDto)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var mitarbeiter = await context.Mitarbeiter.FindAsync(mitarbeiterDto.MitarbeiterId);
                    mitarbeiter.Name = mitarbeiterDto.Name;
                    mitarbeiter.Vorname = mitarbeiterDto.Vorname;
                    mitarbeiter.Email = mitarbeiterDto.Email;
                    mitarbeiter.IsAdmin = mitarbeiterDto.IsAdmin;

                    context.Mitarbeiter.Update(mitarbeiter);
                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich einen Mitarbeiter bearbeitet", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim Bearbeiten eines Mitarbeiters", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }

            }
        }

        // Löscht den Mitarbeiter basierend auf der MitarbeiterId
        public async Task DeleteMitarbeiterByIdAsync(int mitarbeiterId)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var mitarbeiter = context.Mitarbeiter.Single(m => m.MitarbeiterId == mitarbeiterId);
                    context.Mitarbeiter.Remove(mitarbeiter);
                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich einen Mitarbeiter gelöscht", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim löschen eines Mitarbeiters", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }

        // Erstellt für den neuen Mitarbeiter alle MitarbeiterSkill Einträge
        public async Task AddAllSkillsToNewMitarbeiterAsync(int mitarbeiterId)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var allSkills = await context.Skill.ToListAsync();

                    foreach (var skill in allSkills)
                    {
                        var mitarbeiterSkill = new MitarbeiterSkill
                        {
                            MitarbeiterId = mitarbeiterId,
                            SkillId = skill.SkillId,
                            Selbsteinschaetzung = 0
                        };

                        await context.AddAsync(mitarbeiterSkill);
                    }

                    await context.SaveChangesAsync();

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    Serilog.Log.Information("{username} hat erfolgreich alle neuen MitarbeiterSkill Einträge aufgrund eines neuen Mitarbeiters automatisch erfasst", username);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{username}: Es gab einen Fehler beim erfassen neuer MitarbeiterSkill Einträge aufgrund eines neuen Mitarbeiters", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }
    }
}
