using AutoMapper;
using ISLWebService.Dto;
using ISLWebService.Models;
using ISLWebService.Security;

namespace ISLWebService.Profiles
{
    public class UtentiProfile: Profile
    {
        public UtentiProfile()
        {
            CreateMap<Utenti, UtentiDto>()
                .ForMember
                (
                    dest => dest.Ruoli,
                    opt => opt.MapFrom(src => src.UtentiRuoli.Select(ur => ur.Ruolo.Descrizione).ToList())
                )
                .ForMember
                (
                    dest => dest.Magazzino,
                    opt => opt.MapFrom(src => src.CodiceMagazzino)
                );

            CreateMap<Ruoli, RuoliDto>();

        }
    }
}
