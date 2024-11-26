using System;
using AutoMapper;
using CatalogService.Contracts;
using CatalogService.Models;

namespace CatalogService.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateCategoryRequest, Category>()
        .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<UpdateCategoryRequest, Category>()
        .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<CreateItemRequest, Item>()
        .ForMember(dest => dest.ItemId, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<UpdateItemRequest, Item>()
        .ForMember(dest => dest.ItemId, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
