using AutoMapper;
using Factor.Application.DTOs;
using Factor.Infrastructure.Persistence.Models;
using Factor.Persistence.Entities.Persistence.Models;

namespace Factor.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}