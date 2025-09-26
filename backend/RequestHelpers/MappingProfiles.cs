using AutoMapper;
using backend.DTOs;
using backend.Entities;

namespace backend.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<LeadDto, Lead>().ReverseMap();
        CreateMap<CreateLeadDto, Lead>();
        CreateMap<UpdateLeadDto, Lead>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
