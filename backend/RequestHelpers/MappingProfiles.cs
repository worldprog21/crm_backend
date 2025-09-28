using AutoMapper;
using backend.DTOs;
using backend.Entities;

namespace backend.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Leads
        CreateMap<LeadDto, Lead>().ReverseMap();
        CreateMap<CreateLeadDto, Lead>();
        CreateMap<UpdateLeadDto, Lead>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Customers
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Contacts
        CreateMap<Contact, ContactDto>().ReverseMap();
        CreateMap<CreateContactDto, Contact>();
        CreateMap<UpdateContactDto, Contact>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
