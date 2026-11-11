
using AutoMapper;
using SparePackaging.Domain.Models;
using SparePackaging.Application.Dtos;
using SparePackaging.Application.ViewModels;

namespace SparePackaging.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Domain Model <-> DTO
        CreateMap<VendorItem, VendorItemDto>().ReverseMap();

        // Domain Model -> ViewModel
        CreateMap<VendorItem, VendorItemVm>();
        // DTO -> ViewModel
        CreateMap<VendorItemDto, VendorItemVm>();
    }
}
