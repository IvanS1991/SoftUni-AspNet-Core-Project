namespace PropertyAds.WebApp.Infrastructure
{
    using AutoMapper;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.Property;
    using PropertyAds.WebApp.Models.PropertyAggregate;
    using PropertyAds.WebApp.Models.Watchlist;
    using PropertyAds.WebApp.Services.ConversationServices;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Services.UserServices;
    using PropertyAds.WebApp.Services.WatchlistServices;
    using System.Linq;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Property, PropertyServiceModel>()
                .ForMember(p => p.ImageIds, m => m
                    .MapFrom(x => x.Images.Select(i => i.Id)))
                .ForMember(p => p.Owner, m => m
                    .MapFrom(x => x.Owner.Email));

            this.CreateMap<District, DistrictServiceModel>();
            this.CreateMap<PropertyType, PropertyTypeServiceModel>();
            this.CreateMap<PropertyAggregate, PropertyAggregateServiceModel>();
            this.CreateMap<PropertyImage, PropertyImageServiceModel>();
            this.CreateMap<User, UserServiceModel>();
            this.CreateMap<Conversation, ConversationServiceModel>()
                .ForMember(p => p.Messages, c => c
                    .MapFrom(x => x.Messages.OrderBy(m => m.CreatedOn)));
            this.CreateMap<Message, MessageServiceModel>();

            this.CreateMap<PropertyServiceModel, PropertyDetailsViewModel>()
                .ForMember(p => p.District, c => c
                    .MapFrom(x => x.District.Name))
                .ForMember(p => p.Type, c => c
                    .MapFrom(x => x.Type.Name));
            this.CreateMap<PropertyServiceModel, PropertySummaryViewModel>()
                .ForMember(p => p.ImageId, c => c
                    .MapFrom(x => x.ImageIds.Count() > 0 ? x.ImageIds.First() : null))
                .ForMember(p => p.PropertyTypeName, c => c
                    .MapFrom(x => x.Type.Name));
            this.CreateMap<PropertyServiceModel, PropertyFormModel>();

            this.CreateMap<PropertyAggregateServiceModel, PropertyAggregateViewModel>();

            this.CreateMap<Watchlist, WatchlistServiceModel>()
                .ForMember(x => x.WatchlistPropertySegments, c => c
                    .MapFrom(s => s.WatchlistPropertySegments.OrderBy(wps => wps.PropertyType.SortRank)));
            this.CreateMap<WatchlistProperty, WatchlistPropertyServiceModel>();
            this.CreateMap<WatchlistPropertySegment, WatchlistPropertySegmentServiceModel>();
            this.CreateMap<WatchlistServiceModel, WatchlistDetailsViewModel>();
        }
    }
}
