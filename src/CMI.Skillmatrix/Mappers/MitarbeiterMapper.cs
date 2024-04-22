using CMI.Skillmatrix.Components.Data.Models;
using CMI.Skillmatrix.Components.DTOs;

namespace CMI.Skillmatrix.Mappers
{
    public class MitarbeiterMapper
    {
        public static Mitarbeiter MapToMitarbeiter(MitarbeiterDTO mitarbeiterDto)
        {
            if (mitarbeiterDto == null)
            {
                return null;
            }

            return new Mitarbeiter
            {
                Name = mitarbeiterDto.Name,
                Vorname = mitarbeiterDto.Vorname,
                Email = mitarbeiterDto.Email,
                IsAdmin = mitarbeiterDto.IsAdmin
            };
        }

        public static MitarbeiterDTO MapToMitarbeiterDTO(Mitarbeiter mitarbeiter)
        {
            if (mitarbeiter == null)
            {
                return null;
            }

            return new MitarbeiterDTO()
            {
                MitarbeiterId = mitarbeiter.MitarbeiterId,
                Name = mitarbeiter.Name,
                Vorname = mitarbeiter.Vorname,
                Email = mitarbeiter.Email,
                IsAdmin = mitarbeiter.IsAdmin
            };
        }
    }
}
