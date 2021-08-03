namespace PropertyAds.WebApp.Tests.Mocks
{
    using AutoMapper;
    using PropertyAds.WebApp.Infrastructure;

    public static class MapperMock
    {
        public static IMapper Instance()
        {
            var mapperConfig = new MapperConfiguration(x => x.AddProfile<MappingProfile>());

            return new Mapper(mapperConfig);
        }
    }
}
