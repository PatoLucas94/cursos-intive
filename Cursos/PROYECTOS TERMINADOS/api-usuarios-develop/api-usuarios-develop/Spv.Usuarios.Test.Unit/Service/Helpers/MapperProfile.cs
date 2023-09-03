using AutoMapper;
using Spv.Usuarios.Api.MappingProfiles;

namespace Spv.Usuarios.Test.Unit.Service.Helpers
{
    public static class MapperProfile
    {
        public static IMapper GetAppProfile()
        {
            var mappingProfile = new AppMappingProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));

            return new Mapper(config);
        }
    }
}