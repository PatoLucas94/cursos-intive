using AutoMapper;
using Spv.Usuarios.Api.ViewModels.SSOController.Output;
using Spv.Usuarios.Api.ViewModels.UsuarioController.CommonUsuario.Output;
using Spv.Usuarios.Common.Dtos.SSORepository.Output;
using Spv.Usuarios.Common.Dtos.UsuariosService.Input;
using Spv.Usuarios.Common.Dtos.UsuariosService.Output;
using Spv.Usuarios.Domain.Entities.V2;
using Spv.Usuarios.Service.Helpers;

namespace Spv.Usuarios.Api.MappingProfiles
{
    /// <summary>
    /// Mapping profile
    /// </summary>
    public class AppMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppMappingProfile()
        {
            CreateMap<UsuarioV2, PerfilMigradoModelOutput>();
            CreateMap<PerfilMigradoModelOutput, PerfilMigradoModelResponse>();
            CreateMap<ValidacionExistenciaModelInput, ActualizarPersonIdModelInput>();
            CreateMap<ObtenerUsuarioModelInput, ActualizarPersonIdModelInput>();
            CreateMap<TokenModelOutput, TokenModelResponse>();

            CreateMap<PerfilModelOutput, ObtenerUsuarioModelResponse>()
                .ForMember(
                    dest => dest.Identifier,
                    opt => opt.MapFrom(src =>
                        UserEncoding.Base64Encode(src.DocumentNumber, src.Username, src.Canal, false)
                    )
                );
        }
    }
}
