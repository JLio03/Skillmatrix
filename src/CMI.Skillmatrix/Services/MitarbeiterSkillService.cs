using CMI.Skillmatrix.Components.Data;
using CMI.Skillmatrix.Components.Data.Models;
using CMI.Skillmatrix.Components.DTOs;
using CMI.Skillmatrix.Components.Shared;
using CMI.Skillmatrix.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CMI.Skillmatrix.Services
{
    public class MitarbeiterSkillService(IDbContextFactory<SkillmatrixDbContext> dbContextFactory, IHttpContextAccessor contextAccessor)
    {
        // gibt alle SkillkategorieMitSelbsteinschaetzungDTOs mit der entsprechenden Liste an SkillMitSelbsteinschaetzungDTO zurück
        public async Task<List<SkillkategorieMitSelbseinschaetzungDTO>> GetSkillkategorieWithSkillsWithSelbsteinschaetzungAsync(int mitarbeiterId)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                var mitarbeiterSkills = await context.MitarbeiterSkill
                    .Include(ms => ms.Skill)
                    .ThenInclude(s => s.Skillkategorie)
                    .Where(ms => ms.MitarbeiterId == mitarbeiterId)
                    .ToListAsync();

                var groupedSkills = mitarbeiterSkills
                    .GroupBy(ms => ms.Skill.Skillkategorie)
                    .Select(g => new SkillkategorieMitSelbseinschaetzungDTO
                    {
                        SkillkategorieId = g.Key.SkillkategorieId,
                        SkillkategorieName = g.Key.SkillkategorieName,
                        SkillsMitSelbsteinschaetzung = g.Select(ms => new SkillMitSelbsteinschaetzungDTO
                            {
                                SkillId = ms.Skill.SkillId,
                                SkillName = ms.Skill.SkillName,
                                Selbsteinschaetzung = ms.Selbsteinschaetzung
                            })
                            .ToList()
                    })
                    .ToList();

                return groupedSkills;
            }
        }

        // gibt alle Mitarbeiter zurück, die der Filterkriterien entsprechen
        public async Task<List<MitarbeiterDTO>> GetMitarbeiterBySkillAndRatingAsync(List<FilterModel> filters)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                var mitarbeiterQuery = context.Mitarbeiter.AsQueryable();

                foreach (var filter in filters)
                {
                    if (!string.IsNullOrWhiteSpace(filter.SkillName) && filter.MinRating.HasValue)
                    {
                        int? maxRating = filter.MaxRating ?? int.MaxValue;

                        mitarbeiterQuery = from m in mitarbeiterQuery
                            join ms in context.MitarbeiterSkill on m.MitarbeiterId equals ms.MitarbeiterId
                            join s in context.Skill on ms.SkillId equals s.SkillId
                            where s.SkillName == filter.SkillName
                                  && ms.Selbsteinschaetzung >= filter.MinRating.Value
                                  && ms.Selbsteinschaetzung <= maxRating
                            select m;
                    }
                }

                var filteredMitarbeiters = await mitarbeiterQuery.Distinct().ToListAsync();

                var result = filteredMitarbeiters.Select(m => new MitarbeiterDTO
                {
                    MitarbeiterId = m.MitarbeiterId,
                    Name = m.Name,
                    Vorname = m.Vorname,
                    Email = m.Email,
                    IsAdmin = m.IsAdmin
                }).ToList();

                return result;
            }
        }

        // Updated die Selbsteinschätzung basierend auf der MitarbeiterId und SkillId
        public async Task UpdateSelbsteinschaetzungAsync(int mitarbeiterId, int skillId, int selbsteinschaetzung)
        {
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var mitarbeiterSkill = await context.MitarbeiterSkill
                        .FirstOrDefaultAsync(ms => ms.MitarbeiterId == mitarbeiterId && ms.SkillId == skillId);

                    if (mitarbeiterSkill != null)
                    {
                        mitarbeiterSkill.Selbsteinschaetzung = selbsteinschaetzung;
                        await context.SaveChangesAsync();
                    }

                    // Logging
                    var username = contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                    var mitarbeiter = $"{mitarbeiterSkill.Mitarbeiter.Vorname} {mitarbeiterSkill.Mitarbeiter.Name}";
                    Serilog.Log.Information("{username} hat erfolgreich die Selbsteinschätzung von {mitarbeiter} bearbeitet", username, mitarbeiter);
                }
                catch (Exception e)
                {
                    // Logging
                    Serilog.Log.Error(e, "{Username}: Es gab einen Fehler beim bearbeiten der Selbsteinschätzung", contextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown");
                    throw;
                }
            }
        }
    }
}
