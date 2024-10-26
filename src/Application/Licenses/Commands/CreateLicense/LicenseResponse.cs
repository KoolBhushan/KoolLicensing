using KoolLicensing.Application.Customers.Queries;
using KoolLicensing.Domain.Entities;
using KoolLicensing.Domain.Enums;

namespace KoolLicensing.Application.Licenses.Commands.CreateLicense;

public record LicenseResponse
{
    public string Key { get; set; } = string.Empty;

    public LicenseType LicenseType {  get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public DateTimeOffset Expires { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<License, LicenseResponse>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key!.Value))
                .ForMember(dest => dest.LicenseType, opt => opt.MapFrom(src => src.LicenseType))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name))
                .ForMember(dest => dest.Expires, opt => opt.MapFrom(src => src.Expires))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer!.Name));
        }
    }
}
