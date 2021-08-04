﻿namespace PropertyAds.WebApp.Infrastructure
{
    using AutoMapper;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.Property;
    using PropertyAds.WebApp.Models.PropertyAggregate;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using System.Linq;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Property, PropertyServiceModel>()
                .ForMember(p => p.ImageIds, m => m
                    .MapFrom(x => x.Images.Select(i => i.Id)))
                .ForMember(p => p.District, m => m
                    .MapFrom(x => x.District.Name))
                .ForMember(p => p.Type, m => m
                    .MapFrom(x => x.Type.Name))
                .ForMember(p => p.Owner, m => m
                    .MapFrom(x => x.Owner.Email));

            this.CreateMap<District, DistrictServiceModel>();
            this.CreateMap<PropertyType, PropertyTypeServiceModel>();
            this.CreateMap<PropertyAggregate, PropertyAggregateServiceModel>();
            this.CreateMap<PropertyImage, PropertyImageServiceModel>();

            this.CreateMap<PropertyServiceModel, PropertyDetailsViewModel>();
            this.CreateMap<PropertyServiceModel, PropertySummaryViewModel>()
                .ForMember(p => p.ImageId, c => c
                    .MapFrom(x => x.ImageIds.Count() > 0 ? x.ImageIds.First() : null));

            this.CreateMap<PropertyAggregateServiceModel, PropertyAggregateViewModel>()
                .ForMember(p => p.PropertyType, c => c
                    .MapFrom(x => x.PropertyType.Name))
                .ForMember(p => p.District, c => c
                    .MapFrom(x => x.District.Name));
        }
    }
}