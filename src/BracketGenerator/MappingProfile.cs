using AutoMapper;
using BracketGenerator.Models;

namespace BracketGenerator
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, Team>()
                .ForMember(dest => dest.TeamId, opt => opt.MapFrom(country => country.Id))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(country => country.Group))
                .ForMember(dest => dest.SeedNo, opt => opt.MapFrom(country => country.Seed));
        }
    }
}
