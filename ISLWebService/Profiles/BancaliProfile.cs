using AutoMapper;
using ISLWebService.Dto;
using ISLWebService.Models;

namespace ISLWebService.Profiles
{
    public class BancaliProfile: Profile
    {
        public BancaliProfile()
        {
            CreateMap<Bancali, BancaliDto>();
        }
    }
}
