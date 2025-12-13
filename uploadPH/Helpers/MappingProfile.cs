using AutoMapper;
using Pharmacy.Models;
using Pharmacy.Models.Dto;

namespace Pharmacy.Helpers
{
    public class MappingProfile: Profile 
    {
        public MappingProfile() 
        {
            CreateMap<Drug, DrugSummaryDto>()
             .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()));
            CreateMap<DrugCreateDto, Drug>()
    .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
    .ForMember(dest => dest.Tags, opt => opt.Ignore());
            CreateMap<DrugUpdateDto, Drug>()
            .ForMember(dest => dest.Tags, opt => opt.Ignore());


          
        }
    }
}
