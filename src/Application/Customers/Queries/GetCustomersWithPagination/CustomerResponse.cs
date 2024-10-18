using KoolLicensing.Application.Products.Queries;
using KoolLicensing.Domain.Entities;

namespace KoolLicensing.Application.Customers.Queries;

public record CustomerResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Customer, CustomerResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName));
        }
    }
}
